using HRsystem.Api.Database;
using HRsystem.Api.Features.EmployeeRequest.EmployeeVacation;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.ExceptionHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
    public record EmployeeAnnualBalance() : IRequest<EmployeeAnnualBalanceDto>;

    public class EmployeeAnnualBalanceDto
    {
        public decimal? UsedBalance { get; set; }

        public decimal? RemainBalance { get; set; }

    }
    public class EmployeeAnnualBalanceHandler : IRequestHandler<EmployeeAnnualBalance, EmployeeAnnualBalanceDto>
    {
        private readonly DBContextHRsystem _db;

        private readonly ICurrentUserService _currentUser;


        public EmployeeAnnualBalanceHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<EmployeeAnnualBalanceDto> Handle(EmployeeAnnualBalance request, CancellationToken ct)
        {
            var employeeId = _currentUser.EmployeeID;
            var employee = await _db.TbEmployees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employee == null) throw new NotFoundException("Employee Not Found", employeeId);


            var balance = await _db.TbEmployeeVacationBalances
                .FirstOrDefaultAsync(b => b.EmployeeId == employee.EmployeeId && b.VacationTypeId == 1, ct);

            return new EmployeeAnnualBalanceDto
            {
                UsedBalance = balance.UsedDays,
                RemainBalance = balance.RemainingDays,
            };

        }

    }
}
