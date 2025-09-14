using System.Security.Claims;

namespace HRsystem.Api.Services.CurrentUser
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }
        string UserLanguage { get; }
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId =>
            Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        public string? UserName =>
            _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string UserLanguage
        {
            get
            {
                var lang = _httpContextAccessor.HttpContext?
                    .Request.Headers["Accept-Language"]
                    .FirstOrDefault();

                // normalize to "en" or "ar"
                if (string.IsNullOrWhiteSpace(lang))
                    return "en";

                // Handle cases like "en-US" or "ar-EG"
                return lang.Split('-')[0].ToLower();
            }
        }
    }
}
