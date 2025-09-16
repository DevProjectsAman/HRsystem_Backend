using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HRsystem.Api.Features.Employee.DTO;
using System;
using HRsystem.Api.Database;
using HRsystem.Api.Shared.DTO;

namespace HRsystem.Api.Features.Employee
{
    public record UpdateEmployeeCommand(EmployeeUpdateDto Employee) : IRequest<ResponseResultDTO>;

    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, ResponseResultDTO>
    {
        private readonly DBContextHRsystem _db;

        public UpdateEmployeeHandler(DBContextHRsystem db) => _db = db;

        public async Task<ResponseResultDTO> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.Employee;

                var employee = await _db.TbEmployees
                    .FirstOrDefaultAsync(e => e.EmployeeId == dto.EmployeeId, cancellationToken);

                if (employee == null)
                {
                    return new ResponseResultDTO
                    {
                        Success = false,
                        Message = $"Employee with Id {dto.EmployeeId} was not found."
                    };
                }

                // 🔹 Update only non-null values from DTO
                employee.EmployeeCodeHr = dto.EmployeeCodeHr ?? employee.EmployeeCodeHr;
                employee.FirstName = dto.FirstName ?? employee.FirstName;
                employee.LastName = dto.LastName ?? employee.LastName;
                employee.Birthdate = dto.Birthdate ?? employee.Birthdate;
                employee.HireDate = dto.HireDate ?? employee.HireDate;
                employee.NationalId = dto.NationalId ?? employee.NationalId;
                employee.PassportNumber = dto.PassportNumber ?? employee.PassportNumber;
                employee.JobTitleId = dto.JobTitleId ;
                employee.CompanyId = dto.CompanyId;
                employee.DepartmentId = dto.DepartmentId ?? employee.DepartmentId;
                employee.ManagerId = dto.ManagerId ?? employee.ManagerId;
                employee.Email = dto.Email ?? employee.Email;
                employee.PrivateMobile = dto.PrivateMobile ?? employee.PrivateMobile;
                employee.BuisnessMobile = dto.BuisnessMobile;
                employee.Gender = dto.Gender;
                employee.Status = dto.Status ?? employee.Status;

                employee.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync(cancellationToken);

                return new ResponseResultDTO
                {
                    Success = true,
                    Message = "Employee updated successfully."
                };
            }
            catch (DbUpdateException dbEx)
            {
                // Handle FK violations gracefully
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = $"Database update failed: {dbEx.InnerException?.Message ?? dbEx.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }
        }
    }
}
