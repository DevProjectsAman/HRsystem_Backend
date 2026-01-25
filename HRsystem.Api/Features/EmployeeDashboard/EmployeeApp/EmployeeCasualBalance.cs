using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
    public record EmployeeCasualBalance() : IRequest<EmployeeCasualBalanceDto>;

    public class EmployeeCasualBalanceDto
    {
        public decimal? UsedBalance { get; set; }

        public decimal? RemainBalance { get; set; }

    }
    public class EmployeeCasualBalanceHandler : IRequestHandler<EmployeeCasualBalance, EmployeeCasualBalanceDto>
    {
        private readonly DBContextHRsystem _db;

        private readonly ICurrentUserService _currentUser;


        public EmployeeCasualBalanceHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<EmployeeCasualBalanceDto> Handle(EmployeeCasualBalance request, CancellationToken ct)
        {
            var employeeId = _currentUser.EmployeeID;
            var employee = await _db.TbEmployees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employee == null) throw new Exception($"Employee Not Found ID={employeeId}");


            var balance = await _db.TbEmployeeVacationBalances
                .FirstOrDefaultAsync(b => b.EmployeeId == employee.EmployeeId && b.VacationTypeId == 6, ct);

            return new EmployeeCasualBalanceDto
            {
                UsedBalance = balance.UsedDays,
                RemainBalance = balance.RemainingDays,
            };

        }

    }
}
