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


namespace HRsystem.Api.Services
{
    public class JwtService 
    {
        private readonly IConfiguration _configuration;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly DBContextHRsystem _dbContext;

        // ✅ FIX: Constructor parameter name was incorrect
        public JwtService(IConfiguration configuration, RoleManager<ApplicationRole> roleManager, DBContextHRsystem dbContext)
        {
            _configuration = configuration;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        // ✅ FIX: Add 'async' keyword to method signature
        public async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user, IList<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            

            var claims = new List<Claim>
    {
              


        // Standard JWT claims
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),              // subject (user ID)
        
        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),    // email
        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty), // username
        new Claim("PermissionVersion", user.PermissionVersion.ToString()),
        new Claim("eid", SimpleCrypto.Encrypt(user.EmployeeId.ToString())),
        new Claim("cid", SimpleCrypto.Encrypt(user.CompanyId.ToString()))
    };

            // Add roles + permissions
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); // short role claim

                var permissions = await GetRolePermissionsAsync(role);
                if (permissions != null)
                {
                    //claims.Add(new Claim("permission", JsonSerializer.Serialize(permissions)));

                    foreach (var permission in permissions)
                    {
                        claims.Add(new Claim("permission", permission)); // short permission claim
                    }
                }
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return token;
        }

        private async Task<List<string>> GetRolePermissionsAsync(string role)
        {
            var roleDetails = await _roleManager.FindByNameAsync(role);
            if (roleDetails == null) return new List<string>();

            return await (from rolePermission in _dbContext.AspRolePermissions
                          join permission in _dbContext.AspPermissions
                          on rolePermission.PermissionId equals permission.PermissionId
                          where rolePermission.RoleId == roleDetails.Id
                          select permission.PermissionName).ToListAsync();
        }
    }
}

