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

    public CreateEmployeeHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
    {
        _db = db;
        _currentUserService = currentUserService;
    }

    public async Task<NewEmployeeIdDTO> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Employee;
        var currentEmployeeId = _currentUserService.EmployeeID;

        using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // 1. create Employee 
            var employee = new TbEmployee
            {
                // ✅ Basic Data
                EnglishFullName = dto.EmployeeBasicData.EnglishFullName,
                ArabicFullName = dto.EmployeeBasicData.ArabicFullName,
                NationalId = dto.EmployeeBasicData.NationalId,
                Birthdate = dto.EmployeeBasicData.Birthdate,
                PlaceOfBirth = dto.EmployeeBasicData.PlaceOfBirth,
                Gender = dto.EmployeeBasicData.Gender,
                PassportNumber = dto.EmployeeBasicData.PassportNumber,
                MaritalStatusId = dto.EmployeeBasicData.MaritalStatusId,
                NationalityId = dto.EmployeeBasicData.NationalityId,
                Email = dto.EmployeeBasicData.Email,
                PrivateMobile = dto.EmployeeBasicData.PrivateMobile,
                BuisnessMobile = dto.EmployeeBasicData.BuisnessMobile,
                Address = dto.EmployeeBasicData.Address,
                EmployeePhotoPath = dto.EmployeeBasicData.EmployeePhotoPath,
                Note = dto.EmployeeBasicData.Note,

                // ✅ Organization Data
                CompanyId = dto.EmployeeOrganization.CompanyId,
                DepartmentId = dto.EmployeeOrganization.DepartmentId,
                JobTitleId = dto.EmployeeOrganization.JobTitleId,
                ManagerId = dto.EmployeeOrganization.ManagerId,
                ContractTypeId = dto.EmployeeOrganization.ContractTypeId,
                SerialMobile = dto.EmployeeOrganization.SerialMobile,
                Status = dto.EmployeeOrganization.Status,
                EmployeeCodeFinance = dto.EmployeeOrganization.EmployeeCodeFinance,
                EmployeeCodeHr = dto.EmployeeOrganization.EmployeeCodeHr,
                HireDate = dto.EmployeeOrganization.HireDate,
                StartDate = dto.EmployeeOrganization.StartDate,
                EndDate = dto.EmployeeOrganization.EndDate,

                // ✅ Work Conditions
                ShiftId = dto.EmployeeWorkConditions.ShiftId,
                WorkDaysId = dto.EmployeeWorkConditions.WorkDaysId,

                // ✅ Meta
                CreatedAt = DateTime.Now,
                CreatedBy = (int)currentEmployeeId,
                UpdatedBy = (int)currentEmployeeId,
                UpdatedAt = DateTime.Now,
            };

            _db.TbEmployees.Add(employee);
            await _db.SaveChangesAsync(cancellationToken);

            // 2. Work Locations
            if (dto.EmployeeWorkConditions.EmployeeWorkLocations != null && dto.EmployeeWorkConditions.EmployeeWorkLocations.Any())
            {
                var workLocations = dto.EmployeeWorkConditions.EmployeeWorkLocations.Select(loc => new TbEmployeeWorkLocation
                {
                    EmployeeId = employee.EmployeeId,
                    CityId = loc.CityId,
                    WorkLocationId = loc.WorkLocationId,
                    CompanyId = loc.CompanyId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = (int)currentEmployeeId
                }).ToList();

                _db.TbEmployeeWorkLocations.AddRange(workLocations);
            }

            // 3. Vacation Balances
            if (dto.EmployeeVacationBalances != null && dto.EmployeeVacationBalances.Any())
            {
                var balances = dto.EmployeeVacationBalances.Select(bal => new TbEmployeeVacationBalance
                {
                    EmployeeId = employee.EmployeeId,
                    VacationTypeId = bal.VacationTypeId,
                    Year = bal.Year,
                    TotalDays = bal.TotalDays,
                    UsedDays = bal.UsedDays,
                    RemainingDays = bal.RemainingDays
                }).ToList();

                _db.TbEmployeeVacationBalances.AddRange(balances);
            }

            await _db.SaveChangesAsync(cancellationToken);

            // ✅ Commit transaction
            await transaction.CommitAsync(cancellationToken);

            return new NewEmployeeIdDTO
            {
                EmployeeId = employee.EmployeeId
            };
        }
        catch (Exception)
        {
           
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

