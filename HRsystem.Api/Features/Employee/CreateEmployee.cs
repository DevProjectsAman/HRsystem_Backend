using HRsystem.Api.Database.DataTables;
using MediatR;
using HRsystem.Api.Features.Employee.DTO;
using HRsystem.Api.Database;
using HRsystem.Api.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using HRsystem.Api.Services.CurrentUser;

public record CreateEmployeeCommand(EmployeeCreateDto Employee) : IRequest<NewEmployeeIdDTO>;

public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, NewEmployeeIdDTO>
{
    private readonly DBContextHRsystem _db;

    private readonly ICurrentUserService _currentUserService;

    public CreateEmployeeHandler(DBContextHRsystem db, ICurrentUserService currentUserService) { _db = db; _currentUserService = currentUserService; }

    public async Task<NewEmployeeIdDTO> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Employee;
        var HRempolyeeID = _currentUserService.EmployeeID;

        var employee = new TbEmployee
        {
           EmployeeCodeFinance = dto.EmployeeCodeFinance,
            EmployeeCodeHr = dto.EmployeeCodeHr,
            FirstName = dto.FirstName,
            ArabicFirstName = dto.ArabicFirstName,
            LastName = dto.LastName,
            ArabicLastName = dto.ArabicLastName,
            Birthdate = dto.Birthdate,
            HireDate = dto.HireDate,
            Gender = dto.Gender,
            NationalId = dto.NationalId,
            PassportNumber = dto.PassportNumber,
            PlaceOfBirth = dto.PlaceOfBirth,
            BloodGroup = dto.BloodGroup,
            JobTitleId = dto.JobTitleId,
            CompanyId = dto.CompanyId,
            DepartmentId = dto.DepartmentId,
            ManagerId = dto.ManagerId,
            ShiftId = dto.ShiftId,
            MaritalStatusId = dto.MaritalStatusId,
            NationalityId = dto.NationalityId,
            Email = dto.Email,
            PrivateMobile = dto.PrivateMobile,
            BuisnessMobile = dto.BuisnessMobile,
            SerialMobile = dto.SerialMobile,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            IsTopmanager = dto.IsTopManager,
            IsFulldocument = dto.IsFullDocument,
            Note = dto.Note,
            Status = dto.Status ?? "Active", // default if not provided
            CreatedAt = DateTime.UtcNow,
            CreatedBy = (int)HRempolyeeID, // TODO: inject ICurrentUserService if you want the logged-in user ID
            UpdatedBy = (int)HRempolyeeID,
            UpdatedAt = DateTime.UtcNow,
        };

        _db.TbEmployees.Add(employee);
        await _db.SaveChangesAsync(cancellationToken);

        return new NewEmployeeIdDTO
        {
            EmployeeId = employee.EmployeeId
        };
    }
}
