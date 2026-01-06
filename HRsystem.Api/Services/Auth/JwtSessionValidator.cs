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
    public record UserSecurityProfile(
    string CurrentJti,
    int PermissionVersion,
    bool ForceLogout
);

    public class JwtSessionValidator
    {
        private readonly DBContextHRsystem _db;
        private readonly IMemoryCache _cache;
        private readonly ICurrentUserService _currentUser;
        private readonly ISecurityCacheService _securityCache;

        public JwtSessionValidator(
            DBContextHRsystem db,
            IMemoryCache cache,
            ICurrentUserService currentUser,
            ISecurityCacheService securityCache)
        {
            _db = db;
            _cache = cache;
            _currentUser = currentUser;
            _securityCache = securityCache;
        }

        public async Task ValidateAsync(TokenValidatedContext context)
        {
            var principal = context.Principal;

            // 1. Extract Claims from Token
            var userIdStr = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            var tokenJti = principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);
            var tokenPermVersion = principal?.FindFirstValue("PermissionVersion");

            if (string.IsNullOrEmpty(userIdStr) || string.IsNullOrEmpty(tokenJti) || string.IsNullOrEmpty(tokenPermVersion))
            {
                context.Fail("Required security claims are missing.");
                return;
            }

            int userId = int.Parse(userIdStr);
          //  string cacheKey = $"user_sec_{userId}_{_currentUser.X_ClientType}";

            // Use the helper for the key!
            string cacheKey = _securityCache.GetCacheKey(userId, _currentUser.X_ClientType);

            // 2. Try to get Security Profile from Cache (Fast Path)
            if (!_cache.TryGetValue(cacheKey, out UserSecurityProfile? profile))
            {
                // 3. Cache Miss: Query DB (Slow Path)
                // We fetch the User and the Active Session in one go
                var securityData = await (from u in _db.Users
                                          join s in _db.TbUserSession
                                          on u.Id equals s.UserId
                                          where u.Id == userId
                                          && s.ClientType == _currentUser.X_ClientType
                                          && s.IsActive == true
                                          select new UserSecurityProfile(
                                              s.Jti,
                                              u.PermissionVersion,
                                              u.ForceLogout
                                          )).FirstOrDefaultAsync();

                if (securityData == null)
                {
                    context.Fail("Session not found or user deactivated.");
                    return;
                }

                profile = securityData;

                // Store in cache for 5 minutes (adjust as needed)
                _cache.Set(cacheKey, profile, TimeSpan.FromMinutes(5));
            }

            // 4. Validate Logic (Order matters for performance)

            // 🔥 Check Force Logout
            if (profile!.ForceLogout)
            {
                context.Fail("Your account has been remotely logged out.");
                return;
            }

            // 🔥 Check Permission Version
            if (profile.PermissionVersion.ToString() != tokenPermVersion)
            {
                context.Fail("Your permissions have changed. Please login again.");
                return;
            }

            // 🔥 Check Single Session (JTI check)
            if (profile.CurrentJti != tokenJti)
            {
                context.Fail("This account logged in from another device.");
                return;
            }

            // 5. Throttled LastSeen Update
            await UpdateLastSeenThrottled(userId, tokenJti);
        }

        private async Task UpdateLastSeenThrottled(int userId, string jti)
        {
            string lastSeenCacheKey = $"last_seen_{jti}";

            // Only update the database once every 5 minutes per session
            if (!_cache.TryGetValue(lastSeenCacheKey, out _))
            {
                var session = await _db.TbUserSession
                    .FirstOrDefaultAsync(s => s.UserId == userId && s.Jti == jti);

                if (session != null)
                {
                    session.LastSeenAt = DateTime.UtcNow;
                    await _db.SaveChangesAsync();

                    // Set a "lock" in cache so we don't update DB again for 5 minutes
                    _cache.Set(lastSeenCacheKey, true, TimeSpan.FromMinutes(5));
                }
            }
        }
    }
}