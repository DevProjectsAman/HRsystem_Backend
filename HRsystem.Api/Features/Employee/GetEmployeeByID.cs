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
    public record GetEmployeeByIdQuery(int EmployeeId) : IRequest<ResponseResultDTO<EmployeeReadDto?>>;

    // Simple validator to ensure ID is valid
    public class GetEmployeeByIdQueryValidator : AbstractValidator<GetEmployeeByIdQuery>
    {
        public GetEmployeeByIdQueryValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("EmployeeId must be greater than zero.");
        }
    }

    // HRsystem.Api.Features.Employee.GetEmployeeByIdHandler.cs

        public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, ResponseResultDTO<EmployeeReadDto?>>
        {
            private readonly DBContextHRsystem _db;
            private readonly ICurrentUserService _currentUser;

            public GetEmployeeByIdHandler(DBContextHRsystem db, ICurrentUserService currentUser)
            {
                _db = db;
                _currentUser = currentUser;
            }

            public async Task<ResponseResultDTO<EmployeeReadDto?>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    // load employee with related lookups that we will show (keep list minimal to avoid big joins)
                    var e = await _db.TbEmployees
                        .AsNoTracking()
                        .Include(x => x.JobTitle)
                        .Include(x => x.Company)
                        .Include(x => x.Manager)
                        .Include(x => x.Department)
                        .Include(x => x.Nationality)
                        .Include(x => x.Shifts)          // if navigation property exists; otherwise remove
                        .Include(x => x.MaritalStatus)  // if navigation property exists; otherwise remove
                        .FirstOrDefaultAsync(x => x.EmployeeId == request.EmployeeId, cancellationToken);

                    if (e == null)
                        return new ResponseResultDTO<EmployeeReadDto?> { Success = false, Message = "Employee not found." };

                    // helpers to convert sbyte? tinyint flags to bool?
                    bool? ToBool(sbyte? v) => v.HasValue ? (v.Value != 0) : (bool?)null;

                    // fetch translated names using your helper (works for json-string or LocalizedData)
                    string jobTitleName = e.JobTitle?.TitleName?.GetTranslation(_currentUser.UserLanguage ?? "en") ?? string.Empty;
                    string companyName = e.Company?.CompanyName ?? string.Empty;
                    string deptName = e.Department?.DepartmentName?.GetTranslation(_currentUser.UserLanguage ?? "en");
                    string managerName = e.Manager != null ? $"{e.Manager.EnglishFullName}" : null;
                    string nationalityName = e.Nationality?.NameEn.ToString();
                    string shiftName = e.Shifts?.ShiftName.GetTranslation(_currentUser.UserName); // if shift has no localization, use as-is
                    string maritalName = e.MaritalStatus?.NameAr;

                var dto = new EmployeeReadDto
                {
                    EmployeeCodeFinance = e.EmployeeCodeFinance,
                    EmployeeCodeHr = e.EmployeeCodeHr,
                    //EnglishFullName = e.EnglishFullName;
                    //ArabicFullName = e.ArabicFullName;
                    Birthdate = e.Birthdate,
                    HireDate = e.HireDate,
                    Gender = e.Gender,
                    NationalId = e.NationalId,
                    PassportNumber = e.PassportNumber,
                    PlaceOfBirth = e.PlaceOfBirth,
                    BloodGroup = e.BloodGroup,
                    JobTitleId = e.JobTitleId,
                    CompanyId = e.CompanyId,
                    DepartmentId = e.DepartmentId,
                    ManagerId = e.ManagerId,
                    ShiftId = e.ShiftId,
                    MaritalStatusId = e.MaritalStatusId,
                    NationalityId = e.NationalityId,
                    Email = e.Email,
                    PrivateMobile = e.PrivateMobile,
                    BuisnessMobile = e.BuisnessMobile,
                    SerialMobile = e.SerialMobile,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    //IsTopmanager = e.IsTopmanager,
                    //IsFulldocument = e.IsFulldocument,
                    Note = e.Note,
                    Status = e.Status,
                    CreatedAt = e.CreatedAt,
                    CreatedBy = e.CreatedBy,
                    UpdatedBy = e.UpdatedBy,
                    UpdatedAt = e.UpdatedAt,
                };

                    return new ResponseResultDTO<EmployeeReadDto?> { Success = true, Data = dto };
                }
                catch (Exception ex)
                {
                    // Keep message friendly, include raw in logs (or inner error for debugging)
                    // You likely have GlobalExceptionMiddleware but we still handle gracefully here
                    return new ResponseResultDTO<EmployeeReadDto?>
                    {
                        Success = false,
                        Message = "Failed to read employee details: " + ex.Message
                    };
                }
            }
        }
    }


