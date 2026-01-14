using System.Security.Cryptography;
using System.Text;

namespace HRsystem.Api.Shared.Tools
{
    public static class GenerateRefreshTokenHelperr
    {
        public static string GenerateRefreshToken()
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            return refreshToken;
        }

        public static string HashRefreshToken(string RefreshToken)
        {
            string refreshTokenHash = BitConverter.ToString(SHA256.HashData(Encoding.UTF8.GetBytes(RefreshToken))).Replace("-", "").ToLowerInvariant();
            return refreshTokenHash;
        }


    }
}
