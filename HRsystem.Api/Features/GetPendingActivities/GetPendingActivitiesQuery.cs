using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.GetPendingActivities
{
    //public class GetPendingActivitiesQuery
    //{


        public record GetPendingActivitiesQuery() : IRequest<List<PendingActivityDto>>;
        public class PendingActivityDto
        {
            public long ActivityId { get; set; }
            public string ActivityName { get; set; } = string.Empty;
            public string StatusName { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
        }
        public class GetPendingActivitiesQueryHandler : IRequestHandler<GetPendingActivitiesQuery, List<PendingActivityDto>>
        {
            private readonly DBContextHRsystem _db;
            private readonly ICurrentUserService _currentUserService;
            public GetPendingActivitiesQueryHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
            {
                _db = db;
                _currentUserService = currentUserService;
            }
            public async Task<List<PendingActivityDto>> Handle(GetPendingActivitiesQuery request, CancellationToken ct)
            {
                var employeeId = _currentUserService.EmployeeID;
                var language = _currentUserService.UserLanguage;

                const int PendingStatusId = 7; // غيّر الرقم حسب الـ StatusId بتاع الـ Pending عندك

                return await _db.TbEmployeeActivities
                    .Include(a => a.Status)
                    .Where(a => a.EmployeeId == employeeId && a.StatusId == PendingStatusId)
                    .Select(a => new PendingActivityDto
                    {
                        ActivityId = a.ActivityId,
                        ActivityName = language == "ar"
                        ?a.ActivityType.ActivityName.ar
                        : a.ActivityType.ActivityName.en,   

                        StatusName =  language == "ar"
                        ?a.Status.StatusName.ar
                        : a.Status.StatusName.en,
                        CreatedAt = a.RequestDate
                    })
                    .ToListAsync(ct);
            }
        }
    }
//}
