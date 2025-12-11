using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.mangeractivity
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
        public GetSubordinatesPendingActivitiesQueryHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }
        public async Task<List<EmployeeWithActivitiesDto>> Handle(GetSubordinatesPendingActivitiesQuery request, CancellationToken ct)
        {
            var managerId = _currentUserService.EmployeeID;
            var language = _currentUserService.UserLanguage ?? "en";

            const int PendingStatusId = 10;

            // Step 1: get raw employees + activities from DB
            var employees = await _db.TbEmployees
                .Where(e => e.ManagerId == managerId
                         && e.TbEmployeeActivities.Any(a => a.StatusId == PendingStatusId))
                .Include(e => e.TbEmployeeActivities)
                    .ThenInclude(a => a.ActivityType)
                .Include(e => e.TbEmployeeActivities)
                    .ThenInclude(a => a.Status)
                .Include(e => e.TbEmployeeActivities)
                    .ThenInclude(a => a.Status)
                .ToListAsync(ct);

            // Step 2: map + apply translation in memory
            return employees.Select(e => new EmployeeWithActivitiesDto
            {
                EmployeeId = e.EmployeeId,
                EmployeeName = e.ArabicFullName,
                Activities = e.TbEmployeeActivities
                    .Where(a => a.StatusId == PendingStatusId)
                    .Select(a => new ActivityDto
                    {
                        ActivityId = a.ActivityId,
                        ActivityName = a.ActivityType.ActivityName.GetTranslation(language), // ✅ now translated
                        StatusName = a.Status.StatusName.GetTranslation(language),           // ✅ now translated
                        CreatedAt = a.RequestDate
                    })
                    .ToList()
            }).ToList();
        }



    }
}
