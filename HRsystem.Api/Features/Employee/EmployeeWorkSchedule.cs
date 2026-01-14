using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Database;
using HRsystem.Api.Shared.ExceptionHandling;
using MediatR;
using HRsystem.Api.Features.Employee.DTO;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Employee
{
    public class EmployeeWorkSchedule
    {
        // ✅ Command هنا جوا نفس الـ class
        public record EmployeeWorkScheduleCommand(int EmployeeId, EmployeeWorkScheduleDto UpdatedData) : IRequest<TbEmployee>;

        // ✅ Handler هنا كمان
        public class UpdateEmployeeWorkScheduleHandler : IRequestHandler<EmployeeWorkScheduleCommand, TbEmployee>
        {
            private readonly DBContextHRsystem _db;

            public UpdateEmployeeWorkScheduleHandler(DBContextHRsystem db) => _db = db;

            public async Task<TbEmployee> Handle(EmployeeWorkScheduleCommand request, CancellationToken cancellationToken)
            {
                var employee = await _db.TbEmployees
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId, cancellationToken);

                if (employee == null)
                    throw new Exception($"Invalid Employee Id:{ request.EmployeeId}");

                // ✅ تحديث الحقول من DTO
                employee.EmployeeCodeFinance = request.UpdatedData.EmployeeCodeFinance;
                employee.ShiftId = request.UpdatedData.ShiftId;
                employee.RemoteWorkDaysId = request.UpdatedData.RemoteWorkDaysId;
                employee.WorkDaysId = request.UpdatedData.WorkDaysId;

                _db.TbEmployees.Update(employee);
                await _db.SaveChangesAsync(cancellationToken);

                return employee; // ✅ هيرجع object
            }
        }
    }
}


