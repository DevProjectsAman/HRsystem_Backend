using HRsystem.Api.Database.Entities;
using HRsystem.Api.Shared.EncryptText;
using HRsystem.Api.Shared.Tools; // where SimpleCrypto lives
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HRsystem.Api.Services.CurrentUser
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }
        string UserLanguage { get; }

        int? EmployeeID { get; }
        int? CompanyID { get; }
        string? DeviceId { get; }   // ✅ NEW
        string? X_ClientType { get; }   // ✅ NEW

        string? IPAddress { get; }

    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
         

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
 



        public int UserId =>
            Convert.ToInt32(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        public string? UserName =>
            User?.Identity?.Name;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        public string UserLanguage
        {
            get
            {
                var lang = _httpContextAccessor.HttpContext?
                    .Request.Headers["Accept-Language"]
                    .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(lang))
                    return "en";

                return lang.Split('-')[0].ToLower();
            }
        }

        public int? EmployeeID
        {
            get
            {
                var encrypted = User?.FindFirst("eid")?.Value;
                if (string.IsNullOrEmpty(encrypted))
                    return null;

                try
                {
                    var decrypted = SimpleCrypto.Decrypt(encrypted);
                    return int.TryParse(decrypted, out var id) ? id : null;
                }
                catch
                {
                    return null;
                }
            }
        }

        public int? CompanyID
        {
            get
            {
                var encrypted = User?.FindFirst("cid")?.Value;
                if (string.IsNullOrEmpty(encrypted))
                    return null;

                try
                {
                    var decrypted = SimpleCrypto.Decrypt(encrypted);
                    return int.TryParse(decrypted, out var id) ? id : null;
                }
                catch
                {
                    return null;
                }
            }
        }


        public string? DeviceId
        {
            get
            {
                var res=
                 _httpContextAccessor.HttpContext?
                    .Request.Headers["X-Device-Id"]
                    .FirstOrDefault();

                return res ?? string.Empty;

            }
        }
        public string? X_ClientType
        {
            get
            {
                var res = _httpContextAccessor.HttpContext?
                    .Request.Headers["X-ClientType"]
                    .FirstOrDefault();

                return res ?? string.Empty;
            }
        }


        public string? IPAddress
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                if (context == null) return null;

                var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                string? ip = !string.IsNullOrWhiteSpace(forwardedFor)
                             ? forwardedFor.Split(',').FirstOrDefault()?.Trim()
                             : context.Connection.RemoteIpAddress?.ToString();

                if (ip == "::1" || ip == "127.0.0.1")
                    return "localhost";  // optional normalization

                return ip;
            }
        }
    }
}
