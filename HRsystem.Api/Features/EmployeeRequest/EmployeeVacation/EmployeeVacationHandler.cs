using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.ExceptionHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;

namespace HRsystem.Api.Features.EmployeeRequest.EmployeeVacation
{
    public record RequestVacationCommand(int VacationTypeId, DateOnly StartDate, DateOnly EndDate, int DaysCount) : IRequest<EpmloyeeVacationDto>;

    public class RequestVacationHandler : IRequestHandler<RequestVacationCommand, EpmloyeeVacationDto>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public RequestVacationHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<EpmloyeeVacationDto> Handle(RequestVacationCommand request, CancellationToken ct)
        {
            var employeeId = _currentUser.EmployeeID;
            var employee = await _db.TbEmployees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employee == null)
                throw new NotFoundException("Employee Not Found", employeeId);

            var balance = await _db.TbEmployeeVacationBalances
                .FirstOrDefaultAsync(b => b.EmployeeId == employee.EmployeeId && b.VacationTypeId == request.VacationTypeId, ct);

            if (balance == null)
                throw new NotFoundException("VacationTypeId not found", request.VacationTypeId);

            if (balance.RemainingDays < request.DaysCount)
                throw new NotFoundException("Balance not enough", request.DaysCount);

            await using var transaction = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                // ✅ Step 1: Create Activity
                var activity = new TbEmployeeActivity
                {
                    EmployeeId = employee.EmployeeId,
                    ActivityTypeId = request.VacationTypeId,
                    StatusId = 7, // Pending
                    RequestBy = employee.EmployeeId,
                    RequestDate = DateTime.Now,
                    CompanyId = employee.CompanyId
                };
                _db.TbEmployeeActivities.Add(activity);
                await _db.SaveChangesAsync(ct); // ✅ دلوقتي ActivityId اتسجل في DB

                // ✅ Step 2: Create Vacation مرتبط بالـ Activity
                var vacation = new TbEmployeeVacation
                {
                    ActivityId = activity.ActivityId, // لازم يكون موجود من DB
                    VacationTypeId = request.VacationTypeId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    DaysCount = request.DaysCount,
                };
                _db.TbEmployeeVacations.Add(vacation);

                var check = await _db.TbVacationTypes.FirstOrDefaultAsync(e => e.VacationTypeId == request.VacationTypeId, ct);

                if (check.IsDeductable == true)
                {
                    // ✅ Step 3: Update Balance
                    balance.UsedDays += request.DaysCount;
                    balance.RemainingDays -= request.DaysCount;
                }
                await _db.SaveChangesAsync(ct);

                // ✅ Step 4: Commit
                await transaction.CommitAsync(ct);

                return new EpmloyeeVacationDto
                {
                    VacationId = vacation.VacationId,
                    ActivityId = vacation.ActivityId,
                    StartDate = vacation.StartDate,
                    EndDate = vacation.EndDate,
                    VacationTypeId = vacation.VacationTypeId,
                    DaysCount = vacation.DaysCount
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

    }


    public record GetVacationBalanceCommand(int VacationTypeId) : IRequest<EmployeeVacationBalanceDto>;

    public class GetVacationBalanceHandler : IRequestHandler<GetVacationBalanceCommand, EmployeeVacationBalanceDto>
    {
        private readonly DBContextHRsystem _db;

        private readonly ICurrentUserService _currentUser;


        public GetVacationBalanceHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<EmployeeVacationBalanceDto> Handle(GetVacationBalanceCommand request, CancellationToken ct)
        {
            var employeeId = _currentUser.EmployeeID;
            var employee = await _db.TbEmployees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employee == null) throw new NotFoundException("Employee Not Found", employeeId);


            var balance = await _db.TbEmployeeVacationBalances
                .FirstOrDefaultAsync(b => b.EmployeeId == employee.EmployeeId && b.VacationTypeId == request.VacationTypeId, ct);

            if (balance == null) throw new NotFoundException("VacationTypeId not found", request.VacationTypeId);

            return new EmployeeVacationBalanceDto
            {
                BalanceId = balance.BalanceId,
                EmployeeId = balance.EmployeeId,
                VacationTypeId = balance.VacationTypeId,
                Year = balance.Year,
                TotalDays = balance.TotalDays,
                UsedDays = balance.UsedDays,
                RemainingDays = balance.RemainingDays
            };
        }
    }

}
