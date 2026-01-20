using HRsystem.Api.Database;
using HRsystem.Api.Services.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using HRsystem.Api.Shared.Tools;
using HRsystem.Api.Database.Entities;

namespace HRsystem.Api.Features.AccessManagment.RefreshTokens
{
    public record RefreshTokenCommand(
        string RefreshToken,
        string ClientType,      // ✅ Pass explicitly, not from ICurrentUserService
        string? DeviceId = null // ✅ Optional device ID
    ) : IRequest<AuthResponseDto>;

    public record AuthResponseDto(
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpiresAt
    );

    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly DBContextHRsystem _db;
        private readonly JwtService _jwtService;

        public RefreshTokenHandler(
            DBContextHRsystem db,
            JwtService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> Handle(
      RefreshTokenCommand request,
      CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var refreshHash = GenerateRefreshTokenHelperr.HashRefreshToken(request.RefreshToken);

            // 1️⃣ Find session by refresh token
            var session = await _db.TbUserSession
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.RefreshTokenHash == refreshHash &&
                    x.ClientType == request.ClientType,
                    cancellationToken);

            if (session == null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            // 2️⃣ USER / SESSION HARD INVALIDATION
            if (session.User == null ||
                session.User.ForceLogout ||
                !session.IsActive ||
                session.RefreshTokenExpiresAt < now)
            {
                await DeleteAllUserSessions(
                    session.UserId,
                    request.ClientType,
                    cancellationToken);

                throw new UnauthorizedAccessException("Session expired or invalidated.");
            }

            // 3️⃣ TOKEN REUSE DETECTION (Grace Period)
            if (session.RevokedAt.HasValue)
            {
                // Within grace window (30s)
                if (session.RevokedAt.Value.AddSeconds(30) > now)
                {
                    var nextSession = await _db.TbUserSession
                        .FirstOrDefaultAsync(x =>
                            x.RefreshTokenHash == session.ReplacedByTokenHash,
                            cancellationToken);

                    if (nextSession != null)
                    {
                        var jwt = await _jwtService.GenerateTokenAsync(
                            session.User,
                            session.Jti);

                        return new AuthResponseDto(
                            AccessToken: new JwtSecurityTokenHandler().WriteToken(jwt),
                            RefreshToken: "USE_EXISTING_NEW_TOKEN",
                            AccessTokenExpiresAt: jwt.ValidTo
                        );
                    }
                }

                // ❌ Reuse outside grace window → SECURITY BREACH
                await DeleteAllUserSessions(
                    session.UserId,
                    request.ClientType,
                    cancellationToken);

                throw new UnauthorizedAccessException(
                    "Refresh token reuse detected. All sessions revoked.");
            }

            // 4️⃣ CLEANUP OLD SESSIONS (VERY IMPORTANT)
            // Keep:
            // - current session
            // - revoked sessions within grace window
            await _db.TbUserSession
                .Where(x =>
                    x.UserId == session.UserId &&
                    x.ClientType == request.ClientType &&
                    x.Id != session.Id &&
                    (!x.RevokedAt.HasValue ||
                     x.RevokedAt.Value.AddSeconds(30) < now))
                .ExecuteDeleteAsync(cancellationToken);

            // 5️⃣ ROTATE REFRESH TOKEN
            var newRefreshToken = GenerateRefreshTokenHelperr.GenerateRefreshToken();
            var newRefreshHash = GenerateRefreshTokenHelperr.HashRefreshToken(newRefreshToken);

            // Revoke current session (grace window starts)
            session.RevokedAt = now;
            session.ReplacedByTokenHash = newRefreshHash;
            session.IsActive = false;

            // Create new session (SAME JTI)
            var newSession = new TbUserSession
            {
                UserId = session.UserId,
                ClientType = request.ClientType,
                DeviceId = request.DeviceId,
                Jti = session.Jti, // 🔥 SAME JTI
                RefreshTokenHash = newRefreshHash,
                RefreshTokenExpiresAt = now.AddDays(7),
                CreatedAt = now,
                LastSeenAt = now,
                IsActive = true
            };

            _db.TbUserSession.Add(newSession);

            // 6️⃣ ISSUE NEW ACCESS TOKEN
            var newJwt = await _jwtService.GenerateTokenAsync(
                session.User,
                session.Jti);

            await _db.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto(
                AccessToken: new JwtSecurityTokenHandler().WriteToken(newJwt),
                RefreshToken: newRefreshToken,
                AccessTokenExpiresAt: newJwt.ValidTo
            );
        }


        private async Task DeleteAllUserSessions(    long userId,    string clientType,    CancellationToken cancellationToken)
        {
            await _db.TbUserSession
                .Where(x =>
                    x.UserId == userId &&
                    x.ClientType == clientType)
                .ExecuteDeleteAsync(cancellationToken);
        }


        private async Task InvalidateAllUserSessions(int userId, string clientType, CancellationToken ct)
        {
            var sessions = await _db.TbUserSession
                .Where(s => s.UserId == userId && s.ClientType == clientType)
                .ToListAsync(ct);

            foreach (var s in sessions) s.IsActive = false;
            await _db.SaveChangesAsync(ct);
        }

    }

    public static class RefreshTokenEndpoint
    {
        public static void MapRefreshTokenEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/auth/refresh-token",
                async (RefreshTokenRequest request, IMediator mediator) =>
                {
                    var command = new RefreshTokenCommand(
                        request.RefreshToken,
                        request.ClientType,
                        request.DeviceId
                    );

                    var result = await mediator.Send(command);
                    return Results.Ok(result);
                })
            .AllowAnonymous() // ✅ CRITICAL: No authentication required
            .RequireRateLimiting("RefreshTokenPolicy")
            .WithTags("Auth")
            .WithName("RefreshToken")
            .WithSummary("Refresh JWT using a valid refresh token.")
            .WithDescription("Generates a new JWT and refresh token pair. No authentication required.")
            .Produces<AuthResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }

    // DTO for the request
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string ClientType { get; set; } = string.Empty; // "web" or "mobile"
        public string? DeviceId { get; set; }
    }
}