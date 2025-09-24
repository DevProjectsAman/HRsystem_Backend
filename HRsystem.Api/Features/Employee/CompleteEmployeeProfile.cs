using HRsystem.Api.Database;
using HRsystem.Api.Features.Employee.DTO;
using HRsystem.Api.Shared.ExceptionHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Employee
{
    public class CompleteEmployeeProfile
    {
        public record CompleteEmployeeProfileCommand(int EmployeeId, CompleteEmployeeProfileDto UpdatedData) : IRequest<bool>;

        public class CompleteEmployeeProfileHandler : IRequestHandler<CompleteEmployeeProfileCommand, bool>
        {
            private readonly DBContextHRsystem _db;

            public CompleteEmployeeProfileHandler(DBContextHRsystem db) => _db = db;

            public async Task<bool> Handle(CompleteEmployeeProfileCommand request, CancellationToken cancellationToken)
            {
                var employee = await _db.TbEmployees
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId, cancellationToken);

                if (employee == null)
                    throw new NotFoundException("Invalid Employee Id:", request.EmployeeId);

                employee.EmployeeCodeFinance = request.UpdatedData.EmployeeCodeFinance;
                employee.JobTitleId = request.UpdatedData.JobTitleId;
                employee.DepartmentId = request.UpdatedData.DepartmentId;
                employee.ManagerId = request.UpdatedData.ManagerId;
                employee.ShiftId = request.UpdatedData.ShiftId;
                employee.MaritalStatusId = request.UpdatedData.MaritalStatusId;
                employee.IsTopmanager = request.UpdatedData.IsTopManager;
                employee.IsFulldocument = request.UpdatedData.IsFullDocument;

                _db.TbEmployees.Update(employee);
                await _db.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
