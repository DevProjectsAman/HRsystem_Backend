using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Lookups.ActivityType.GetAllActivityTypes
{
    public record GetAllActivityTypesQuery() : IRequest<List<ActivityTypeDto>>;

    public class ActivityTypeDto
    {
        public int ActivityTypeId { get; set; }
        public string ActivityTypeName { get; set; }

        public string ActivityTypeCode { get; set; }

        public string ActivityDescription { get; set; }

    }

    public class Handler : IRequestHandler<GetAllActivityTypesQuery, List<ActivityTypeDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser; // service to get lang (optional)

        public Handler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;

        }

        public async Task<List<ActivityTypeDto>> Handle(GetAllActivityTypesQuery request, CancellationToken ct)
        {
            var lang = _currentUser.UserLanguage ?? "en";

            var statuses = await _db.TbActivityTypes.ToListAsync(ct);

            return statuses.Select(s => new ActivityTypeDto
            {
                ActivityTypeId = s.ActivityTypeId,
                ActivityTypeCode = s.ActivityCode,
                ActivityTypeName = s.ActivityName.GetTranslation(lang),// ✅ translated here
                ActivityDescription = s.ActivityDescription
            }).ToList();
            //return await _db.TbActivityTypeDto.ToListAsync(ct);
        }
    }
}
