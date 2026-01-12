using global::HRsystem.Api.Database;
using Microsoft.EntityFrameworkCore;


namespace HRsystem.Api.Services.Auth
{
   
        public interface IRefreshTokenCleanupService
        {
            Task CleanupExpiredSessionsAsync();
        }

        public class RefreshTokenCleanupService : IRefreshTokenCleanupService
        {
            private readonly DBContextHRsystem _db;
            private readonly ILogger<RefreshTokenCleanupService> _logger;

            public RefreshTokenCleanupService(
                DBContextHRsystem db,
                ILogger<RefreshTokenCleanupService> logger)
            {
                _db = db;
                _logger = logger;
            }

            public async Task CleanupExpiredSessionsAsync()
            {
                try
                {
                    var cutoffDate = DateTime.UtcNow;

                    // Delete expired or inactive sessions
                    var expiredSessions = await _db.TbUserSession
                        .Where(s =>
                            (!s.IsActive || s.RefreshTokenExpiresAt < cutoffDate))
                        .ToListAsync();

                    if (expiredSessions.Any())
                    {
                        _db.TbUserSession.RemoveRange(expiredSessions);
                        await _db.SaveChangesAsync();

                        _logger.LogInformation(
                            $"Cleaned up {expiredSessions.Count} expired sessions");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to cleanup expired sessions");
                }
            }
        }
    }

