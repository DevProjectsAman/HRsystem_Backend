using HRsystem.Api.Database;
using HRsystem.Api.Features.EmployeeDashboard.GetPendingActivities;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.GetApprovedActivites
{
    public record GetApprovedActivitiesQueury() : IRequest<List<ApprovedActivityDto>>;

    public class ApprovedActivityDto
    {
        public long ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class GetApprovedActivitiesQueuryHandler : IRequestHandler<GetApprovedActivitiesQueury, List<ApprovedActivityDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;
        public GetApprovedActivitiesQueuryHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }
        public async Task<List<ApprovedActivityDto>> Handle(GetApprovedActivitiesQueury request, CancellationToken ct)
        {
            var employeeId = _currentUserService.EmployeeID;
            var language = _currentUserService.UserLanguage;

            const int ApprovedStatusId = 7; // غيّر الرقم حسب الـ StatusId بتاع الـ Pending عندك

            return await _db.TbEmployeeActivities
                .Include(a => a.Status)
                .Include(a => a.ActivityType)
                .Where(a => a.EmployeeId == employeeId && a.StatusId == ApprovedStatusId)
                .Select(a => new ApprovedActivityDto
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
                .ToListAsync(ct);
        }
    }
}

 
