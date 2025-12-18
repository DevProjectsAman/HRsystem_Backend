using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.ManagerActivity
{
    public record GetNumberOfPendingReqForManager() : IRequest<RequestStatusesOfManagerDto>;

    public class RequestStatusesOfManagerDto
    {
        public int RejectedCount { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
    }

    public class GetNumberOfPendingReqForManagerHandler
        : IRequestHandler<GetNumberOfPendingReqForManager, RequestStatusesOfManagerDto>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;

        public GetNumberOfPendingReqForManagerHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<RequestStatusesOfManagerDto> Handle(GetNumberOfPendingReqForManager request, CancellationToken ct)
        {
            var managerId = _currentUserService.EmployeeID;

            // Define status IDs
            const int PendingStatusId = 10;
            const int ApprovedStatusId = 7;
            const int RejectedStatusId = 8;

            var lastMonthDate = DateTime.UtcNow.AddDays(-30);
            // Get all activities for employees managed by this manager
            var activities = await _db.TbEmployeeActivities
                .Where(a => a.Employee.ManagerId == managerId &&
                            a.RequestDate >= lastMonthDate) 
                .Select(a => a.StatusId)
                .ToListAsync(ct);

            // Count per status
            var result = new RequestStatusesOfManagerDto
            {
                ApprovedCount = activities.Count(a => a == ApprovedStatusId),
                RejectedCount = activities.Count(a => a == RejectedStatusId),
                PendingCount = activities.Count(a => a == PendingStatusId)
            };

            return result;
        }
    }
}

/*
 * using HRsystem.Api.Database;
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
                .ToListAsync(ct);

            // Step 2: map + apply translation in memory
            return employees.Select(e => new EmployeeWithActivitiesDto
            {
                EmployeeId = e.EmployeeId,
                EmployeeName = e.ArabicFirstName + " " + e.ArabicLastName,
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

 * */