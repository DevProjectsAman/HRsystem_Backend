using HRsystem.Api.Database;
using HRsystem.Api.Features.EmployeeDashboard.GetApprovedActivites;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.GetRejectedActivities
{
    public record GetRejectedActivitiesQueury() : IRequest<List<RejectedActivityDto>>;
    public class RejectedActivityDto
    {
        public string EmployeeName { get; set; } = string.Empty;
        public long ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string From { get; set; }
    }
    public class GetRejectedActivitiesQueuryHandler : IRequestHandler<GetRejectedActivitiesQueury, List<RejectedActivityDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;
        public GetRejectedActivitiesQueuryHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }
        public async Task<List<RejectedActivityDto>> Handle(GetRejectedActivitiesQueury request, CancellationToken ct)
        {
            var employeeId = _currentUserService.EmployeeID;
            var language = _currentUserService.UserLanguage;

            const int RejectedStatusId = 8; // غيّر الرقم حسب الـ StatusId بتاع الـ Pending عندك
            var lastMonthDate = DateTime.UtcNow.AddDays(-30);

            var activities = await _db.TbEmployeeActivities
                                     .Include(a => a.Status)
                                     .Include(a => a.Employee)
                                     .Include(a => a.ActivityType)
                                     .Where(a => a.EmployeeId == employeeId
                                              && a.StatusId == RejectedStatusId
                                              && a.RequestDate >= lastMonthDate
                                              && a.ActivityTypeId != 1)
                                     .Select(a => new
                                     {
                                         a.ActivityId,
                                         EmployeeName = a.Employee.EnglishFullName,
                                         ActivityName = language == "ar"
                                             ? a.ActivityType.ActivityName.ar
                                             : a.ActivityType.ActivityName.en,
                                         StatusName = language == "ar"
                                             ? a.Status.StatusName.ar
                                             : a.Status.StatusName.en,
                                         CreatedAt = a.RequestDate
                                     })
                                     .ToListAsync(ct);

            // Now map to DTO and compute 'From' safely in C#
            var result = activities.Select(a => new RejectedActivityDto
            {
                ActivityId = a.ActivityId,
                EmployeeName = a.EmployeeName,
                ActivityName = a.ActivityName,
                StatusName = a.StatusName,
                CreatedAt = a.CreatedAt,
                From = (int)(DateTime.UtcNow - a.CreatedAt).TotalDays == 0
                    ? "Today"
                    : (int)(DateTime.UtcNow - a.CreatedAt).TotalDays == 1
                        ? "1 day"
                        : ((int)(DateTime.UtcNow - a.CreatedAt).TotalDays) + " days"
            })
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

            return result;

        }
    }
}
