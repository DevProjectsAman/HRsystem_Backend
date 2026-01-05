using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Services.CurrentUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
 
namespace HRsystem.Api.Services.Auth
{
    //public class JwtSessionValidator
    //{
    //    private readonly DBContextHRsystem _db;
    //    private readonly UserManager<ApplicationUser> _userManager;
    //    private readonly ICurrentUserService _currentUser;

    //    public JwtSessionValidator(
    //        DBContextHRsystem db,
    //        UserManager<ApplicationUser> userManager,
    //        ICurrentUserService currentUser)
    //    {
    //        _db = db;
    //        _userManager = userManager;
    //        _currentUser = currentUser;
    //    }

    //    public async Task ValidateAsync(TokenValidatedContext context)
    //    {
    //        var principal = context.Principal;

    //        var userIdStr = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
    //        var jti = principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);
    //        var tokenPermissionVersion = principal?.FindFirstValue("PermissionVersion");

    //        if (string.IsNullOrEmpty(userIdStr) ||
    //            string.IsNullOrEmpty(jti) ||
    //            string.IsNullOrEmpty(tokenPermissionVersion))
    //        {
    //            context.Fail("Missing required claims");
    //            return;
    //        }

    //        var userId = int.Parse(userIdStr);

    //        var user = await _userManager.FindByIdAsync(userIdStr);
    //        if (user == null)
    //        {
    //            context.Fail("User not found");
    //            return;
    //        }

    //        // 🔥 ADMIN FORCE LOGOUT
    //        if (user.ForceLogout)
    //        {
    //            context.Fail("User is forced to logout");
    //            return;
    //        }

    //        // 🔥 PERMISSION VERSION
    //        if (user.PermissionVersion.ToString() != tokenPermissionVersion)
    //        {
    //            context.Fail("Stale token");
    //            return;
    //        }

    //        // 🔥 ONE SESSION PER CLIENT TYPE
    //        var session = await _db.TbUserSession
    //            .FirstOrDefaultAsync(s =>
    //                s.UserId == userId &&
    //                s.ClientType == _currentUser.X_ClientType &&
    //                s.Jti == jti &&
    //                s.IsActive);

    //        if (session == null)
    //        {
    //            context.Fail("Session expired or logged out");
    //            return;
    //        }

    //        session.LastSeenAt = DateTime.UtcNow;
    //        await _db.SaveChangesAsync();
    //    }
    //}




public class JwtSessionValidator
    {
        private readonly DBContextHRsystem _db;
        private readonly IMemoryCache _cache;
        private readonly ICurrentUserService _currentUser;

        public JwtSessionValidator(DBContextHRsystem db, IMemoryCache cache, ICurrentUserService currentUser)
        {
            _db = db;
            _cache = cache;
            _currentUser = currentUser;
        }

        public async Task ValidateAsync(TokenValidatedContext context)
        {
            var principal = context.Principal;
            var userIdStr = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            var jti = principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);

            if (string.IsNullOrEmpty(userIdStr) || string.IsNullOrEmpty(jti))
            {
                context.Fail("Unauthorized");
                return;
            }

            // 1. Generate a unique cache key for this user and client type
            string cacheKey = $"active_session_{userIdStr}_{_currentUser.X_ClientType}";

            // 2. Try to get the active JTI from Memory Cache
            if (!_cache.TryGetValue(cacheKey, out string? cachedJti))
            {
                // 3. Cache Miss: Hit the database
                var session = await _db.TbUserSession
                    .Where(s => s.UserId == int.Parse(userIdStr) &&
                                s.ClientType == _currentUser.X_ClientType &&
                                s.IsActive)
                    .Select(s => s.Jti)
                    .FirstOrDefaultAsync();

                if (session == null || session != jti)
                {
                    context.Fail("Session invalidated by another login");
                    return;
                }

                // Store in cache for 10 minutes
                cachedJti = session;
                _cache.Set(cacheKey, cachedJti, TimeSpan.FromMinutes(10));
            }

            // 4. Validate JTI match
            if (cachedJti != jti)
            {
                context.Fail("Session expired");
                return;
            }

            // 5. Throttled Database Update (Optional)
            // Only update LastSeenAt if the last update was more than 5 mins ago
            await ThrottleLastSeenUpdate(int.Parse(userIdStr), jti);
        }

        private async Task ThrottleLastSeenUpdate(int userId, string jti)
        {
            string updateKey = $"last_seen_update_{jti}";
            if (!_cache.TryGetValue(updateKey, out _))
            {
                var session = await _db.TbUserSession
                    .FirstOrDefaultAsync(s => s.UserId == userId && s.Jti == jti);

                if (session != null)
                {
                    session.LastSeenAt = DateTime.UtcNow;
                    await _db.SaveChangesAsync();

                    // Don't update the DB again for this session for another 5 minutes
                    _cache.Set(updateKey, true, TimeSpan.FromMinutes(5));
                }
            }
        }
    }



}
