using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Services.LookupCashing;
using HRsystem.Api.Services.VacationCalculation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeRequest.EmployeeVacation
{
    public record RequestVacationCommand(RequestVacationDto VacationRequest)
    : IRequest<EpmloyeeVacationDto>;

    public class RequestVacationHandler : IRequestHandler<RequestVacationCommand, EpmloyeeVacationDto>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;
        private readonly IActivityTypeLookupCache _activityTypeCache;
        private readonly IActivityStatusLookupCache _activityStatusLookupCache;
        private readonly IVacationDaysCalculator _vacationCalculator; // ✅ Add this

        public RequestVacationHandler(
            DBContextHRsystem db,
            ICurrentUserService currentUser,
            IActivityTypeLookupCache activityTypeLookupCache,
            IActivityStatusLookupCache activityStatusLookupCache,
            IVacationDaysCalculator vacationCalculator) // ✅ Add this
        {
            _db = db;
            _currentUser = currentUser;
            _activityTypeCache = activityTypeLookupCache;
            _activityStatusLookupCache = activityStatusLookupCache;
            _vacationCalculator = vacationCalculator; // ✅ Add this
        }

        public async Task<EpmloyeeVacationDto> Handle(RequestVacationCommand request, CancellationToken ct)
        {
            var dto = request.VacationRequest;

            var employeeId = _currentUser.EmployeeID;
            var employee = await _db.TbEmployees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employee == null)
                throw new Exception($"Employee Not Found With ID {employeeId}");

            var balance = await _db.TbEmployeeVacationBalances
                .FirstOrDefaultAsync(b => b.EmployeeId == employee.EmployeeId && b.VacationTypeId == dto.VacationTypeId, ct);

            if (balance == null)
                throw new Exception("Don't Have Vacation Balance for this vacation Type");

            // ✅ Calculate actual working days to deduct
            var calculation = await _vacationCalculator.CalculateActualVacationDaysAsync(
                employee.EmployeeId,
                dto.StartDate,
                dto.EndDate,
                ct);

            if (!calculation.IsValid)
                throw new Exception(calculation.ErrorMessage ?? "Invalid vacation calculation");

            // ✅ Use calculated days instead of dto.DaysCount
            var actualDaysToDeduct = calculation.ActualDaysToDeduct;

            // ✅ Check if there are any working days to deduct
            if (actualDaysToDeduct == 0)
                throw new Exception(
                    $"No working days to deduct. All days in your selected range fall on weekends or holidays. " +
                    $"Breakdown: {calculation.GetSummary()}");

            // ✅ Check balance with actual working days
            if (balance.RemainingDays < actualDaysToDeduct)
                throw new Exception(
                    $"Balance not enough. You requested {dto.DaysCount} calendar days " +
                    $"({actualDaysToDeduct} working days) but your remaining balance is {balance.RemainingDays} days. " +
                    $"Breakdown: {calculation.GetSummary()}");

            await using var transaction = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                var activity = new TbEmployeeActivity
                {
                    EmployeeId = employee.EmployeeId,
                    ActivityTypeId = _activityTypeCache.GetIdByCode(ActivityCodes.VacationRequest),
                    StatusId = _activityStatusLookupCache.GetIdByCode(ActivityStatusCodes.Pending),
                    RequestBy = employee.EmployeeId,
                    RequestDate = DateTime.UtcNow,
                    CompanyId = employee.CompanyId
                };
                _db.TbEmployeeActivities.Add(activity);
                await _db.SaveChangesAsync(ct);

                var vacation = new TbEmployeeVacation
                {
                    ActivityId = activity.ActivityId,
                    VacationTypeId = dto.VacationTypeId,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    DaysCount = (int?)actualDaysToDeduct, // ✅ Store actual working days
                    Notes = dto.Notes,
                };
                _db.TbEmployeeVacations.Add(vacation);

                var vacationType = await _db.TbVacationTypes.FirstOrDefaultAsync(e => e.VacationTypeId == dto.VacationTypeId, ct);

                // ✅ Deduct actual working days from balance
                if (vacationType?.IsDeductable == true)
                {
                    balance.UsedDays = (balance.UsedDays ?? 0) + actualDaysToDeduct;
                    balance.RemainingDays = balance.TotalDays - balance.UsedDays;
                }

                await _db.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);

                return new EpmloyeeVacationDto
                {
                    VacationId = vacation.VacationId,
                    ActivityId = vacation.ActivityId,
                    StartDate = vacation.StartDate,
                    EndDate = vacation.EndDate,
                    VacationTypeId = vacation.VacationTypeId,
                    DaysCount = (int?)actualDaysToDeduct,
                    Notes = vacation.Notes,
                };
            }
            catch
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
            if (employee == null) throw new Exception($"Employee Not Found {employeeId}");

            var balance = await _db.TbEmployeeVacationBalances
                .FirstOrDefaultAsync(b => b.EmployeeId == employee.EmployeeId && b.VacationTypeId == request.VacationTypeId, ct);

            if (balance == null) throw new Exception($"Don't Have Vacation Balance From This Type {request.VacationTypeId}");

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