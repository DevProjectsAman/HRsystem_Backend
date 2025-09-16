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
                    string managerName = e.Manager != null ? $"{e.Manager.FirstName} {e.Manager.LastName}" : null;
                    string nationalityName = e.Nationality?.NameEn.ToString();
                    string shiftName = e.Shifts?.ShiftName.GetTranslation(_currentUser.UserName); // if shift has no localization, use as-is
                    string maritalName = e.MaritalStatus?.NameAr;

                    var dto = new EmployeeReadDto(
                        EmployeeId: e.EmployeeId,
                        EmployeeCodeFinance: e.EmployeeCodeFinance,
                        EmployeeCodeHr: e.EmployeeCodeHr,
                        JobTitleId:(int) e.JobTitleId,
                        JobTitleName: jobTitleName,
                        FirstName: e.FirstName,
                        ArabicFirstName: e.ArabicFirstName,
                        LastName: e.LastName,
                        ArabicLastName: e.ArabicLastName,
                        HireDate: e.HireDate,
                        Birthdate: e.Birthdate,
                        Gender: e.Gender,
                        NationalId: e.NationalId,
                        PassportNumber: e.PassportNumber,
                        PlaceOfBirth: e.PlaceOfBirth,
                        BloodGroup: e.BloodGroup,
                        ManagerId: e.ManagerId,
                        ManagerName: managerName,
                        CompanyId: e.CompanyId,
                        CompanyName: companyName,
                        CreatedBy: e.CreatedBy,
                        CreatedAt: e.CreatedAt,
                        UpdatedBy: e.UpdatedBy,
                        UpdatedAt: e.UpdatedAt,
                        PrivateMobile: e.PrivateMobile,
                        BuisnessMobile: e.BuisnessMobile,
                        Email: e.Email,
                        SerialMobile: e.SerialMobile,
                        StartDate: e.StartDate,
                        EndDate: e.EndDate,
                        IsTopManager: ToBool(e.IsTopmanager),
                        IsFullDocument: ToBool(e.IsFulldocument),
                        Note: e.Note,
                        Status: e.Status,
                        NationalityId: e.NationalityId,
                        NationalityName: nationalityName,
                        DepartmentId: e.DepartmentId,
                        DepartmentName: deptName,
                        ShiftId: e.ShiftId,
                        ShiftName: shiftName,
                        MaritalStatusId: e.MaritalStatusId,
                        MaritalStatusName: maritalName
                    );

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


