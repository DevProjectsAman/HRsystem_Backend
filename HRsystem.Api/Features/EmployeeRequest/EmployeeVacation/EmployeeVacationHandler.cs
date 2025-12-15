using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Services.LookupCashing;
using HRsystem.Api.Shared.ExceptionHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;

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

        public RequestVacationHandler(DBContextHRsystem db, ICurrentUserService currentUser
            , IActivityTypeLookupCache activityTypeLookupCache , IActivityStatusLookupCache activityStatusLookupCache)
        {
            _db = db;
            _currentUser = currentUser;
            _activityTypeCache = activityTypeLookupCache;
            _activityStatusLookupCache = activityStatusLookupCache;
        }

        public async Task<EpmloyeeVacationDto> Handle(RequestVacationCommand request, CancellationToken ct)
        {
            var dto = request.VacationRequest;

            var employeeId = _currentUser.EmployeeID;
            var employee = await _db.TbEmployees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employee == null)
                throw new NotFoundException("Employee Not Found", employeeId);

            var balance = await _db.TbEmployeeVacationBalances
                .FirstOrDefaultAsync(b => b.EmployeeId == employee.EmployeeId && b.VacationTypeId == dto.VacationTypeId, ct);

            if (balance == null)
                throw new NotFoundException("Don't Have Vacation Balance From This Type", dto.VacationTypeId);

            if (balance.RemainingDays < dto.DaysCount)
                throw new NotFoundException("Balance not enough", dto.DaysCount);

            await using var transaction = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                var activity = new TbEmployeeActivity
                {
                    EmployeeId = employee.EmployeeId,
                  //  ActivityTypeId = 5,
                    ActivityTypeId = _activityTypeCache.GetIdByCode(ActivityCodes.Attendance),

                  //  StatusId = 7, // Pending

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
                    DaysCount = dto.DaysCount,
                    Notes=dto.Notes,
                };
                _db.TbEmployeeVacations.Add(vacation);

                var check = await _db.TbVacationTypes.FirstOrDefaultAsync(e => e.VacationTypeId == dto.VacationTypeId, ct);

                if (check?.IsDeductable == true)
                {
                    balance.UsedDays += dto.DaysCount;
                    balance.RemainingDays -= dto.DaysCount;
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
                    DaysCount = vacation.DaysCount,
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
            if (employee == null) throw new NotFoundException("Employee Not Found", employeeId);


            var balance = await _db.TbEmployeeVacationBalances
                .FirstOrDefaultAsync(b => b.EmployeeId == employee.EmployeeId && b.VacationTypeId == request.VacationTypeId, ct);

            if (balance == null) throw new NotFoundException(" Don't Have Vacation Balance From This Type ", request.VacationTypeId);

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
