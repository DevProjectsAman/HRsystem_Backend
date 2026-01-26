using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
    public record class GetEmployeeTotalWorkingHoursQueury() : IRequest<decimal>;

    public class GetEmployeeTotalWorkingHoursQueuryHandler : IRequestHandler<GetEmployeeTotalWorkingHoursQueury, decimal>
    {
        private readonly DBContextHRsystem _db;

        private readonly ICurrentUserService _currentUser;


        public GetEmployeeTotalWorkingHoursQueuryHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<decimal> Handle(GetEmployeeTotalWorkingHoursQueury request, CancellationToken ct)
        {

            decimal? TotalHour = null;
            var employeeId = _currentUser.EmployeeID;
            var employeeAtten = await _db.TbEmployeeActivities.Include(a => a.TbEmployeeAttendances)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employeeAtten == null) throw new Exception($"Employee Not Found {employeeId}");


            var balance = await _db.TbEmployeeAttendances
                .FirstOrDefaultAsync(b => b.ActivityId == employeeAtten.ActivityId);

            /* there are more than one attendances for one employeeid and activityid */

            TotalHour = balance.TotalHours;

            if (TotalHour == null) throw new Exception($"TotalHours Not Found{ TotalHour}");

            return (decimal)TotalHour;


        }
    }
}