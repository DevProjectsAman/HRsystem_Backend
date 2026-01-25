using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
    public record class GetEmployeeCheckInLogQueury () : IRequest<DateTime>;

    public class GetEmployeeCheckInLogQueuryHandler : IRequestHandler<GetEmployeeCheckInLogQueury, DateTime>
    {
        private readonly DBContextHRsystem _db;

        private readonly ICurrentUserService _currentUser;


        public GetEmployeeCheckInLogQueuryHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<DateTime> Handle(GetEmployeeCheckInLogQueury request, CancellationToken ct)
        {

            DateTime ? CheckInTime = null;
            var employeeId = _currentUser.EmployeeID;
            var employeeAtten = await _db.TbEmployeeActivities.Include(a => a.TbEmployeeAttendances)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employeeAtten == null) throw new Exception($"Employee Not Found ID= {employeeId}");


            var balance = await _db.TbEmployeeAttendances
                .FirstOrDefaultAsync(b => b.ActivityId == employeeAtten.ActivityId);


            CheckInTime = balance.FirstPuchin;

            if (CheckInTime == null) throw new Exception($"CheckInTime Not Found { CheckInTime}");

            return (DateTime)CheckInTime; 

        }

    }
}
