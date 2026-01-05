using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Services.CurrentUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HRsystem.Api.Services.Auth
{
    public class JwtSessionValidator
    {
        private readonly DBContextHRsystem _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUser;

        public JwtSessionValidator(
            DBContextHRsystem db,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUser)
        {
            _db = db;
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public async Task ValidateAsync(TokenValidatedContext context)
        {
            var principal = context.Principal;

            var userIdStr = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            var jti = principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);
            var tokenPermissionVersion = principal?.FindFirstValue("PermissionVersion");

            if (string.IsNullOrEmpty(userIdStr) ||
                string.IsNullOrEmpty(jti) ||
                string.IsNullOrEmpty(tokenPermissionVersion))
            {
                context.Fail("Missing required claims");
                return;
            }

            var userId = int.Parse(userIdStr);

            var user = await _userManager.FindByIdAsync(userIdStr);
            if (user == null)
            {
                context.Fail("User not found");
                return;
            }

            // 🔥 ADMIN FORCE LOGOUT
            if (user.ForceLogout)
            {
                context.Fail("User is forced to logout");
                return;
            }

            // 🔥 PERMISSION VERSION
            if (user.PermissionVersion.ToString() != tokenPermissionVersion)
            {
                context.Fail("Stale token");
                return;
            }

            // 🔥 ONE SESSION PER CLIENT TYPE
            var session = await _db.TbUserSession
                .FirstOrDefaultAsync(s =>
                    s.UserId == userId &&
                    s.ClientType == _currentUser.X_ClientType &&
                    s.Jti == jti &&
                    s.IsActive);

            if (session == null)
            {
                context.Fail("Session expired or logged out");
                return;
            }

            session.LastSeenAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }






}
