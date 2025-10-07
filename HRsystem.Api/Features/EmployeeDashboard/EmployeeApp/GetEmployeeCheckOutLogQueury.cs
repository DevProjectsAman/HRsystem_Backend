using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.ExceptionHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
    public record class GetEmployeeCheckOutLogQueury() : IRequest<DateTime>;

    public class GetEmployeeCheckOutLogQueuryHandler : IRequestHandler<GetEmployeeCheckOutLogQueury, DateTime>
    {
        private readonly DBContextHRsystem _db;

        private readonly ICurrentUserService _currentUser;


        public GetEmployeeCheckOutLogQueuryHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<DateTime> Handle(GetEmployeeCheckOutLogQueury request, CancellationToken ct)
        {

            DateTime? CheckOutTime = null;
            var employeeId = _currentUser.EmployeeID;
            var employeeAtten = await _db.TbEmployeeActivities.Include(a => a.TbEmployeeAttendances)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employeeAtten == null) throw new NotFoundException("Employee Not Found", employeeId);


            var balance = await _db.TbEmployeeAttendances
                .FirstOrDefaultAsync(b => b.ActivityId == employeeAtten.ActivityId);

            /* there are more than one attendances for one employeeid and activityid */

            CheckOutTime = balance.LastPuchout;

            if (CheckOutTime == null) throw new NotFoundException("CheckOutTime Not Found", CheckOutTime);

            return (DateTime)CheckOutTime;

        }

    }
}

