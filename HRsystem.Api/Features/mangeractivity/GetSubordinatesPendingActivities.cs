using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.mangeractivity
{
    public class EmployeeWithActivitiesDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;

        public List<ActivityDto> Activities { get; set; } = new();
    }

    public class ActivityDto
    {
        public long ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
    public record GetSubordinatesPendingActivitiesQuery() : IRequest<List<EmployeeWithActivitiesDto>>;

    public class GetSubordinatesPendingActivitiesQueryHandler : IRequestHandler<GetSubordinatesPendingActivitiesQuery, List<EmployeeWithActivitiesDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;
        public  GetSubordinatesPendingActivitiesQueryHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db ;
            _currentUserService = currentUserService;
        }
        public async Task<List<EmployeeWithActivitiesDto>> Handle(GetSubordinatesPendingActivitiesQuery request, CancellationToken ct)
        {
            var managerId = _currentUserService.EmployeeID;
            var language = _currentUserService.UserLanguage;

            const int PendingStatusId = 10; // StatusId بتاع الـ Pending

            return await _db.TbEmployees
                .Where(e => e.ManagerId == managerId
                         && e.TbEmployeeActivities.Any(a => a.StatusId == PendingStatusId)) // ✅ يرجع بس الموظفين اللي عندهم Pending
                .Select(e => new EmployeeWithActivitiesDto
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.ArabicFirstName + " " + e.ArabicLastName,
                    Activities = e.TbEmployeeActivities
                        .Where(a => a.StatusId == PendingStatusId)
                        .Select(a => new ActivityDto
                        {
                            ActivityId = a.ActivityId,
                            ActivityName = language == "ar"
                                ? a.ActivityType.ActivityName.ar
                                : a.ActivityType.ActivityName.en,
                            StatusName = language == "ar"
                                ? a.Status.StatusName.ar
                                : a.Status.StatusName.en,
                            CreatedAt = a.RequestDate
                        })
                        .ToList()
                })
                .ToListAsync(ct);

        }
    }
}
