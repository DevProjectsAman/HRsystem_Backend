using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.GetAllActivities
{
    public record GetAllActivitiesQuery() : IRequest<List<AllActivityDto>>;

    public class AllActivityDto
    {
        public string EmployeeName { get; set; } = string.Empty;
        public long ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public string StatusType { get; set; } = string.Empty; // Pending / Approved / Rejected
        public DateTime CreatedAt { get; set; }
        public string From { get; set; } // days from creation
    }

    public class GetAllActivitiesQueryHandler :
        IRequestHandler<GetAllActivitiesQuery, List<AllActivityDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;

        public GetAllActivitiesQueryHandler(
            DBContextHRsystem db,
            ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<List<AllActivityDto>> Handle(
            GetAllActivitiesQuery request, CancellationToken ct)
        {
            var employeeId = _currentUserService.EmployeeID;
            var language = _currentUserService.UserLanguage;
            var lastMonthDate = DateTime.UtcNow.AddDays(-30);

            // Status IDs
            const int Pending = 10;
            const int Approved = 7;
            const int Rejected = 8;

            var activities = await _db.TbEmployeeActivities
                                     .Include(a => a.Status)
                                     .Include(a => a.Employee)
                                     .Include(a => a.ActivityType)
                                     .Where(a =>
                                         a.EmployeeId == employeeId &&
                                         a.RequestDate >= lastMonthDate &&
                                         (a.StatusId == Pending || a.StatusId == Approved || a.StatusId == Rejected) &&
                                         a.ActivityTypeId != 1
                                     )
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
                                         StatusId = a.StatusId,
                                         CreatedAt = a.RequestDate
                                     })
                                     .ToListAsync(ct);

            // Now compute 'From' in C# after EF query
            var result = activities.Select(a => new AllActivityDto
            {
                ActivityId = a.ActivityId,
                EmployeeName = a.EmployeeName,
                ActivityName = a.ActivityName,
                StatusName = a.StatusName,
                StatusType = a.StatusId == Pending ? "Pending" :
                             a.StatusId == Approved ? "Approved" : "Rejected",
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
