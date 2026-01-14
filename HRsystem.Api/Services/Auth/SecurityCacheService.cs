using Microsoft.Extensions.Caching.Memory;

namespace HRsystem.Api.Services.Auth
{
    public interface ISecurityCacheService
    {
        void ClearUserCache(int userId, string clientType);
        string GetCacheKey(int userId, string clientType);
    }

    public class SecurityCacheService : ISecurityCacheService
    {
        private readonly IMemoryCache _cache;
        private const string CachePrefix = "user_sec";

        public SecurityCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Centralized key generation
        public string GetCacheKey(int userId, string clientType)
            => $"{CachePrefix}_{userId}_{clientType}";

        // Clear cache when session changes or permissions update
        public void ClearUserCache(int userId, string clientType)
        {
            _cache.Remove(GetCacheKey(userId, clientType));
        }
    }
}