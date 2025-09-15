 
using HRsystem.Api.Database.DataTables;
using MediatR;
using HRsystem.Api.Features.Employee.DTO;
using System;
using HRsystem.Api.Database;
using HRsystem.Api.Shared.DTO;
using Microsoft.EntityFrameworkCore;

public record CreateEmployeeCommand(EmployeeCreateDto Employee) : IRequest<ResponseResultDTO<NewEmployeeIdDTO>>;

public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, ResponseResultDTO<NewEmployeeIdDTO>>
{
    private readonly  DBContextHRsystem _db;

    public CreateEmployeeHandler(DBContextHRsystem db) => _db = db;

    public async Task<ResponseResultDTO<NewEmployeeIdDTO>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        try
        {

      
        var dto = request.Employee;

            var errors = new List<ResponseErrorDTO>();

            if (!await _db.TbJobTitles.AnyAsync(j => j.JobTitleId == dto.JobTitleId, cancellationToken))
            {
                errors.Add(new ResponseErrorDTO
                {
                    Property = nameof(dto.JobTitleId),
                    Error = $"JobTitleId {dto.JobTitleId} is not valid."
                });
            }

            if (!await _db.TbCompanies.AnyAsync(c => c.CompanyId == dto.CompanyId, cancellationToken))
            {
                errors.Add(new ResponseErrorDTO
                {
                    Property = nameof(dto.CompanyId),
                    Error = $"CompanyId {dto.CompanyId} is not valid."
                });
            }

            if (dto.DepartmentId.HasValue &&
                !await _db.TbDepartments.AnyAsync(d => d.DepartmentId == dto.DepartmentId, cancellationToken))
            {
                errors.Add(new ResponseErrorDTO
                {
                    Property = nameof(dto.DepartmentId),
                    Error = $"DepartmentId {dto.DepartmentId} is not valid."
                });
            }


            // 🔹 If validation failed → return all errors
            if (errors.Any())
            {
                return new ResponseResultDTO<NewEmployeeIdDTO>
                {
                    Success = false,
                    Message = "Validation Error ",
                    Errors = errors // ✅ you can extend your ResponseResultDTO with List<string> Errors
                };
            }


            var employee = new TbEmployee
        {
            EmployeeCodeHr = dto.EmployeeCodeHr,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Birthdate = dto.Birthdate,
            HireDate = dto.HireDate,
            NationalId = dto.NationalId,
            PassportNumber = dto.PassportNumber,
            JobTitleId = dto.JobTitleId,
            CompanyId = dto.CompanyId,
            DepartmentId = dto.DepartmentId,
            ManagerId = dto.ManagerId,
            Email = dto.Email,
            PrivateMobile = dto.PrivateMobile,
            BuisnessMobile = dto.BuisnessMobile,
            Gender = dto.Gender,
            Status = "Active",
            CreatedAt = DateTime.UtcNow
        };

        _db.TbEmployees.Add(employee);
        await _db.SaveChangesAsync(cancellationToken);

            // here i need to get the employee id just saved  

            var newEmployeeId = employee.EmployeeId;

            NewEmployeeIdDTO _newEmp = new NewEmployeeIdDTO
            {
                // here i need to get the employee id just saved 
                 EmployeeId = newEmployeeId,
            };


            return new ResponseResultDTO<NewEmployeeIdDTO>
            {
                Success = true,
                Message = "Succ",
                 Data= _newEmp

            };







        }
        catch (Exception ex)
        {

            return new ResponseResultDTO<NewEmployeeIdDTO>
            {
                Success = false ,
                Message = ex.InnerException.Message,
                 

            };
        }

         
    }



}
