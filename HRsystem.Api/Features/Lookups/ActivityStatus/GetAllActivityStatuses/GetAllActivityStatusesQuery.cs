using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Lookups.ActivityStatus.GetAllActivityStatuses
{
    public record GetAllActivityStatusesQuery() : IRequest<List<ActivityStatusDto>>;

    public record ActivityStatusDto
    {
        public int StatusId { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public bool IsFinal { get; set; }
    }

    public class GetAllHandler : IRequestHandler<GetAllActivityStatusesQuery, List<ActivityStatusDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser; // service to get lang (optional)

        public GetAllHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<List<ActivityStatusDto>> Handle(GetAllActivityStatusesQuery request, CancellationToken ct)
        {
            var lang = _currentUser.UserLanguage ?? "en";

            var statuses = await _db.TbActivityStatuses.ToListAsync(ct);

            return statuses.Select(s => new ActivityStatusDto
            {
                StatusId = s.StatusId,
                StatusCode = s.StatusCode,
                StatusName = s.StatusName.GetTranslation(lang), // ✅ translated here
                IsFinal = s.IsFinal
            }).ToList();
        }
    }
}
