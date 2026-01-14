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

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshHash = GenerateRefreshTokenHelperr.HashRefreshToken(request.RefreshToken);

            // 1. Find the session by the hash provided
            var session = await _db.TbUserSession
                .Include(x => x.User)
                .Where(x => x.RefreshTokenHash == refreshHash && x.ClientType == request.ClientType)
                .FirstOrDefaultAsync(cancellationToken);

            if (session == null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            var now = DateTime.UtcNow;

            // 2. CHECK FOR REUSE / GRACE PERIOD
            if (session.RevokedAt.HasValue)
            {
                // GRACE PERIOD: Was it revoked less than 30 seconds ago?
                if (session.RevokedAt.Value.AddSeconds(30) > now)
                {
                    // The user sent an "old" token, but it's within the grace window.
                    // Find the NEW session record that replaced this one to return its token.
                    var nextSession = await _db.TbUserSession
                        .FirstOrDefaultAsync(x => x.RefreshTokenHash == session.ReplacedByTokenHash, cancellationToken);

                    if (nextSession != null)
                    {
                        // We return the info for the NEW token that was already created
                        // Note: You'll need to store the raw token or regenerate the JWT for this specific case
                        var jwtResult = await _jwtService.GenerateTokenAsync(session.User!,session.Jti);
                        return new AuthResponseDto(
                            AccessToken: new JwtSecurityTokenHandler().WriteToken(jwtResult),
                            RefreshToken: "USE_EXISTING_NEW_TOKEN_LOGIC", // See Note Below
                            AccessTokenExpiresAt: jwtResult.ValidTo
                        );
                    }
                }

                // REUSE DETECTION: If we are here, it's a second use OUTSIDE the 30s window.
                await InvalidateAllUserSessions(session.UserId, request.ClientType, cancellationToken);
                throw new UnauthorizedAccessException("Token reuse detected. Security breach suspected.");
            }

            // 3. CHECK EXPIRATION
            if (session.RefreshTokenExpiresAt < now || !session.IsActive || session.User?.ForceLogout == true)
            {
                session.IsActive = false;
                await _db.SaveChangesAsync(cancellationToken);
                throw new UnauthorizedAccessException("Session expired or invalidated.");
            }

            // 4. ROTATION (The "Correct" way with Grace Period)
            var newRefreshToken = GenerateRefreshTokenHelperr.GenerateRefreshToken();
            var newHash = GenerateRefreshTokenHelperr.HashRefreshToken(newRefreshToken);

            // Mark current session as Revoked but keep it for 30 seconds
            session.RevokedAt = now;
            session.ReplacedByTokenHash = newHash;
            session.IsActive = false; // It's no longer the "primary" active token

            // Create a NEW session record for the new token
            var newSession = new TbUserSession
            {
                UserId = session.UserId,
                RefreshTokenHash = newHash,
                RefreshTokenExpiresAt = now.AddDays(7), // Sliding window
                ClientType = request.ClientType,
                DeviceId = request.DeviceId,
                IsActive = true,
                CreatedAt = now,
                LastSeenAt = now
            };

            _db.TbUserSession.Add(newSession);

            var jwt = await _jwtService.GenerateTokenAsync(session.User!, session.Jti);
            // newSession.Jti = jwt.Jti;
            // need to keep the same jti for the new session
            newSession.Jti = session.Jti;


            await _db.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto(
                AccessToken: new JwtSecurityTokenHandler().WriteToken(jwt),
                RefreshToken: newRefreshToken,
                AccessTokenExpiresAt: jwt.ValidTo
            );
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