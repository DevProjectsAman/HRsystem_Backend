using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.ExceptionHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
    
    public record EmployeeGetShiftQueury( TimeOnly Time) : IRequest<EmployeeGetShiftDto>;

    public enum statues
    {
        OnTime , 
        Late
    };
    public class EmployeeGetShiftDto
    {
        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public bool ? IsFlexible { get; set; }

        public statues Statues { get; set; }

        public int GracePeriodMinutes { get; set; }

        public TimeOnly? MaxStartTime { get; set; }

    }

    public class EmployeeGetShiftQueuryeHandler : IRequestHandler<EmployeeGetShiftQueury, EmployeeGetShiftDto>
    {
        private readonly DBContextHRsystem _db;

        private readonly ICurrentUserService _currentUser;


        public EmployeeGetShiftQueuryeHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<EmployeeGetShiftDto> Handle(EmployeeGetShiftQueury request, CancellationToken ct)
        {
            var employeeId = _currentUser.EmployeeID;
            var employee = await _db.TbEmployees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employee == null) throw new NotFoundException("Employee Not Found", employeeId);


            var Shift = await _db.TbShifts
                .FirstOrDefaultAsync(b => b.ShiftId == employee.ShiftId , ct);

            var ShiftINfo = new EmployeeGetShiftDto
            {
                EndTime = Shift.EndTime,
                StartTime = Shift.StartTime,
                GracePeriodMinutes = Shift.GracePeriodMinutes,
                IsFlexible = Shift.IsFlexible,
                MaxStartTime = Shift.MaxStartTime,
            };

            switch(Shift.IsFlexible)
            {
                case true:
                    if ((ShiftINfo.MaxStartTime ?? new TimeOnly(0, 0)).AddMinutes(ShiftINfo.GracePeriodMinutes) < TimeOnly.FromDateTime(DateTime.Now)
                        ) 
                        ShiftINfo.Statues = statues.Late;
                    else
                        ShiftINfo.Statues = statues.OnTime;
                    break;
                case false:
                    if ((ShiftINfo.StartTime.AddMinutes(ShiftINfo.GracePeriodMinutes)) <= TimeOnly.FromDateTime(DateTime.Now))
                        ShiftINfo.Statues = statues.Late;
                    else
                        ShiftINfo.Statues = statues.OnTime;
                    break;
            }

            return ShiftINfo;

        }

    }
}
