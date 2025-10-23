using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Features.Employee.DTO;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Employee
{
    // ✅ Query record
    public record GetEmployeeByCodeHrQuery(string EmployeeCodeHr) : IRequest<ResponseResultDTO<EmployeeReadDto?>>;

    // ✅ Validator
    public class GetEmployeeByCodeHrValidator : AbstractValidator<GetEmployeeByCodeHrQuery>
    {
        public GetEmployeeByCodeHrValidator()
        {
            RuleFor(x => x.EmployeeCodeHr)
                .NotEmpty().WithMessage("EmployeeCodeHr is required.");
        }
    }

    // ✅ Handler
    public class GetEmployeeByCodeHrHandler : IRequestHandler<GetEmployeeByCodeHrQuery, ResponseResultDTO<EmployeeReadDto?>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public GetEmployeeByCodeHrHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<ResponseResultDTO<EmployeeReadDto?>> Handle(GetEmployeeByCodeHrQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var e = await _db.TbEmployees
                    .AsNoTracking()
                    .Include(x => x.JobTitle)
                    .Include(x => x.Company)
                    .Include(x => x.Manager)
                    .Include(x => x.Department)
                    .Include(x => x.Nationality)
                    .FirstOrDefaultAsync(x => x.EmployeeCodeHr == request.EmployeeCodeHr, cancellationToken);

                if (e == null)
                    return new ResponseResultDTO<EmployeeReadDto?> { Success = false, Message = "Employee not found." };

                var dto = new EmployeeReadDto
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeCodeFinance = e.EmployeeCodeFinance,
                    EmployeeCodeHr = e.EmployeeCodeHr,
                    EnglishFullName = e.EnglishFullName,
                    ArabicFullName = e.ArabicFullName,
                    Birthdate = e.Birthdate,
                    HireDate = e.HireDate,
                    Email = e.Email,
                    Status = e.Status,
                    DepartmentId = e.DepartmentId,
                    CompanyId = e.CompanyId,
                    JobTitleId = e.JobTitleId
                };

                return new ResponseResultDTO<EmployeeReadDto?> { Success = true, Data = dto };
            }
            catch (Exception ex)
            {
                return new ResponseResultDTO<EmployeeReadDto?>
                {
                    Success = false,
                    Message = "Error retrieving employee: " + ex.Message
                };
            }
        }
    }
}
