using System.Security.Claims;

namespace HRsystem.Api.Services.CurrentUser
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }
    }
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
 
        public Guid UserId =>
        Guid.Parse( _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());


        public string? UserName =>
            _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        
    }



}
