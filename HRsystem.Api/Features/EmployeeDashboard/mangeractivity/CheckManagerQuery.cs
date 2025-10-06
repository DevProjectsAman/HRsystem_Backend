using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.ManagerActivity
{
    public record CheckManagerQuery() : IRequest<bool>;

    public class CheckManagerQueryHandler : IRequestHandler<CheckManagerQuery, bool>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;

        public CheckManagerQueryHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(CheckManagerQuery request, CancellationToken ct)
        {
            var managerId = _currentUserService.EmployeeID;

            // Check if this employee manages at least one employee
            var isManager = await _db.TbEmployees
                .AnyAsync(e => e.ManagerId == managerId, ct);

            return isManager;
        }
    }
}
