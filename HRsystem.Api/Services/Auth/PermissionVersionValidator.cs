using HRsystem.Api.Database.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace HRsystem.Api.Services.Auth
{
  
    public class PermissionVersionValidator<TUser> : ISecurityStampValidator
        where TUser : class
    {
        private readonly UserManager<TUser> _userManager;

        public PermissionVersionValidator(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            var userId = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var claimVersion = context.Principal.FindFirstValue("PermissionVersion");

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(claimVersion))
                return;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return;
            }

            var dbVersion = (user as ApplicationUser)?.PermissionVersion ?? 0;
            if (dbVersion.ToString() != claimVersion)
            {
                // ❌ Token is stale
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            }
        }
    }

}
