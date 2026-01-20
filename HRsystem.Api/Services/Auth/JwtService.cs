using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Shared.EncryptText;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.Design;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace HRsystem.Api.Services.Auth
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DBContextHRsystem _dbContext;

        // ✅ FIX: Constructor parameter name was incorrect
        public JwtService(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, DBContextHRsystem dbContext)
        {
            _configuration = configuration;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        // ✅ FIX: Add 'async' keyword to method signature
        public async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user,string? currentJti)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            // Get expiry time from configuration with fallback
            var expiryInMinutes = int.Parse(jwtSettings["ExpiryInMinutes"] ?? "30");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

           // var jti = Guid.NewGuid().ToString();
           var jti = string.IsNullOrEmpty(currentJti) ? Guid.NewGuid().ToString() : currentJti;

            var claims = new List<Claim>    {
                    // Standard JWT claims
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),              // subject (user ID)

                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),    // email
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty), // username
                           new Claim(JwtRegisteredClaimNames.Jti, jti),
                     new Claim("PermissionVersion", user.PermissionVersion.ToString()),
                    new Claim("eid", SimpleCrypto.Encrypt(user.EmployeeId.ToString())),
                    new Claim("cid", SimpleCrypto.Encrypt(user.CompanyId.ToString()))
                };

            //// Add roles + permissions
            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role)); // short role claim

            //    var permissions = await GetRolePermissionsAsync(role);
            //    if (permissions != null)
            //    {
            //        //claims.Add(new Claim("permission", JsonSerializer.Serialize(permissions)));

            //        foreach (var permission in permissions)
            //        {
            //            claims.Add(new Claim("permission", permission)); // short permission claim
            //        }
            //    }
            //}

            // Collect all permissions   
            // Add roles
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Collect permissions
            var allPermissions = new List<string>();
            foreach (var role in roles)
            {
                var permissions = await GetRolePermissionsAsync(role);
                if (permissions != null)
                {
                    allPermissions.AddRange(permissions);
                }
            }

            // For SystemAdmin, just add the superuser permission
            if (roles.Contains("SystemAdmin"))
            {
                claims.Add(new Claim("permissions", "admin.all")); // or "superuser" or "admin.all"
            }
            else
            {
                // For regular users, add all their permissions
                if (allPermissions.Any())
                {
                    var uniquePermissions = string.Join(",", allPermissions.Distinct());
                    claims.Add(new Claim("permissions", uniquePermissions));
                }
            }


            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
               // expires: DateTime.UtcNow.AddMinutes(10),
                expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
             //  expires: DateTime.UtcNow.AddSeconds(expiryInMinutes),
                signingCredentials: credentials
            );

            return (token);
        }

        private async Task<List<string>> GetRolePermissionsAsync(string role)
        {
            //  var roleDetails = await _roleManager.FindByNameAsync(role);

            var roleDetails = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == role);

            if (roleDetails == null) return new List<string>();

            var rolePerm = await (from rolePermission in _dbContext.AspRolePermissions
                                  join permission in _dbContext.AspPermissions
                                  on rolePermission.PermissionId equals permission.PermissionId
                                  where rolePermission.RoleId == roleDetails.Id
                                  select permission.PermissionName).ToListAsync();

            return rolePerm;
        }
    }
}