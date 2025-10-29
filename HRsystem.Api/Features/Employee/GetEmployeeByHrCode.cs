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
    public record GetEmployeeByCodeHrQuery(string EmployeeCodeHr)
        : IRequest<ResponseResultDTO<EmployeeReadDto?>>;

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
    public class GetEmployeeByCodeHrHandler
        : IRequestHandler<GetEmployeeByCodeHrQuery, ResponseResultDTO<EmployeeReadDto?>>
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
                // 🟢 Load employee with related entities
                var e = await _db.TbEmployees
                    .AsNoTracking()
                    .Include(x => x.JobTitle)
                    .Include(x => x.Company)
                    .Include(x => x.Manager)
                    .Include(x => x.Department)
                    .Include(x => x.Nationality)
                    .Include(x => x.Shifts)
                    .Include(x => x.MaritalStatus)
                    .FirstOrDefaultAsync(x => x.EmployeeCodeHr == request.EmployeeCodeHr, cancellationToken);

                if (e == null)
                {
                    return new ResponseResultDTO<EmployeeReadDto?>
                    {
                        Success = false,
                        Message = "Employee not found."
                    };
                }

                // 🟢 Map to DTO safely
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
                    Status = e.Status?.ToString() ?? "Unknown",

                    // 🔹 Related data with null safety
                    DepartmentId = e.DepartmentId ,
                    DepartmentName = e.Department?.DepartmentName ?? new LocalizedData { en = "N/A", ar = "غير محدد" },

                    CompanyId = e.CompanyId,
                    CompanyName = e.Company?.CompanyName ?? "N/A",

                    JobTitleId = e.JobTitleId,
                    JobTitleName = e.JobTitle?.TitleName ?? new LocalizedData { en = "N/A", ar = "غير محدد" },

                    ShiftId = e.ShiftId,
                    ShiftName = e.Shifts?.ShiftName ?? new LocalizedData { en = "N/A", ar = "غير محدد" },

                    MaritalStatusId = e.MaritalStatusId,
                    MaritalStatusName = e.MaritalStatus?.NameAr ?? "غير محدد",

                    NationalityId = e.NationalityId,
                    NationalityName = e.Nationality?.NameEn ?? "N/A",

                    ManagerId = e.ManagerId,
                    ManagerName = e.Manager?.EnglishFullName ?? "N/A",

                    PrivateMobile = e.PrivateMobile,
                    BuisnessMobile = e.BuisnessMobile,
                    SerialMobile = e.SerialMobile,
                    Note = e.Note,
                    CreatedAt = e.CreatedAt,
                    CreatedBy = e.CreatedBy,
                    UpdatedAt = e.UpdatedAt,
                    UpdatedBy = e.UpdatedBy
                };

                return new ResponseResultDTO<EmployeeReadDto?>
                {
                    Success = true,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseResultDTO<EmployeeReadDto?>
                {
                    Success = false,
                    Message = $"Error retrieving employee: {ex.Message}"
                };
            }
        }
    }
}