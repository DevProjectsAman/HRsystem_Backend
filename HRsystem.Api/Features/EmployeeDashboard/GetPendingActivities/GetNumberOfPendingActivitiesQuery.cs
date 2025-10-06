using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.GetPendingActivities
{

    public record GetNumberOfPendingActivitiesQuery() : IRequest<int>;

    public class GetNumberOfPendingActivitiesQueryHandler : IRequestHandler<GetNumberOfPendingActivitiesQuery, int>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;

        public GetNumberOfPendingActivitiesQueryHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(GetNumberOfPendingActivitiesQuery request, CancellationToken ct)
        {
            var employeeId = _currentUserService.EmployeeID;

            const int PendingStatusId = 10; // عدّلها حسب ID الـ Pending الفعلي عندك

            // يرجع عدد الطلبات فقط
            var pendingCount = await _db.TbEmployeeActivities
                .Where(a => a.EmployeeId == employeeId && a.StatusId == PendingStatusId)
                .CountAsync(ct);

            return pendingCount;
        }
    }
}
