using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.GetPendingActivities
{
    public record GetActivitiesStatusCountQuery() : IRequest<ActivitiesStatusCountDto>;

    public class ActivitiesStatusCountDto
    {
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int PendingCount { get; set; }
    }

    public class GetActivitiesStatusCountQueryHandler
        : IRequestHandler<GetActivitiesStatusCountQuery, ActivitiesStatusCountDto>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;

        public GetActivitiesStatusCountQueryHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<ActivitiesStatusCountDto> Handle(GetActivitiesStatusCountQuery request, CancellationToken ct)
        {
            var employeeId = _currentUserService.EmployeeID;

            // تأكد من IDs الصحيحة للحالات في قاعدة البيانات
            const int ApprovedStatusId = 7;
            const int RejectedStatusId = 8;
            const int PendingStatusId = 10;
            var lastMonthDate = DateTime.UtcNow.AddDays(-30);

            var activities = await _db.TbEmployeeActivities
                .Where(a => a.EmployeeId == employeeId && a.RequestDate >= lastMonthDate)
                .Select(a => a.StatusId)
                .ToListAsync(ct);

            return new ActivitiesStatusCountDto
            {
                ApprovedCount = activities.Count(a => a == ApprovedStatusId),
                RejectedCount = activities.Count(a => a == RejectedStatusId),
                PendingCount = activities.Count(a => a == PendingStatusId)
            };
        }
    }
}
