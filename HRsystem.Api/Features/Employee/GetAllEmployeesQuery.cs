using HRsystem.Api.Database;
using HRsystem.Api.Features.Employee.DTO;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Employee
{
    public record GetAllEmployeesQuery : IRequest<List<EmployeeReadDto>>;

    public class GetAllEmployeesHandler : IRequestHandler<GetAllEmployeesQuery, List<EmployeeReadDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;
        public GetAllEmployeesHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        
        public async Task<List<EmployeeReadDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {

            return await _db.TbEmployees
                .AsNoTracking()
                .Select(e => new EmployeeReadDto
                {
                    /*EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Status = e.Status
                    */

                    EmployeeCodeFinance = e.EmployeeCodeFinance,
                    EmployeeCodeHr = e.EmployeeCodeHr,
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
                    Status = e.Status , // default if not provided
                    CreatedAt = e.CreatedAt,
                    CreatedBy = e.CreatedBy, // TODO: inject ICurrentUserService if you want the logged-in user ID
                    UpdatedBy = e.UpdatedBy,
                    UpdatedAt = e.UpdatedAt,


                })
                .ToListAsync(cancellationToken);
        }
    }

}
