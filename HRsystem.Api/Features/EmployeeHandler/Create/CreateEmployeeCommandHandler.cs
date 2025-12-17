
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Features.EmployeeHandler.Create;

using MediatR;

namespace HRsystem.Api.Features.EmployeeHandler.Create
{
  

     
    public sealed class CreateEmployeeCommandHandler
        : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public CreateEmployeeCommandHandler(
            DBContextHRsystem db,
            ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(
            CreateEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var now = DateTime.UtcNow;
                var userId = _currentUser.UserId;

                #region Create Employee (Aggregate Root)

                var employee = new TbEmployee
                {
                    // ===== Basic Data =====
                    EnglishFullName = request.EmployeeBasicData.EnglishFullName,
                    ArabicFullName = request.EmployeeBasicData.ArabicFullName,
                    NationalId = request.EmployeeBasicData.NationalId,
                    Birthdate = request.EmployeeBasicData.Birthdate,
                    PlaceOfBirth = request.EmployeeBasicData.PlaceOfBirth,
                    Gender = request.EmployeeBasicData.Gender,
                    EmployeePhotoPath = request.EmployeeBasicData.EmployeePhotoPath,

                    // ===== Extra Data =====
                    PassportNumber = request.EmployeeExtraData.PassportNumber,
                    MaritalStatusId = request.EmployeeExtraData.MaritalStatusId,
                    NationalityId = request.EmployeeExtraData.NationalityId,
                    PrivateMobile = request.EmployeeExtraData.PrivateMobile,
                    BuisnessMobile = request.EmployeeExtraData.BuisnessMobile,
                    Email = request.EmployeeExtraData.Email,
                    Address = request.EmployeeExtraData.Address,
                    Religion = request.EmployeeExtraData.Religion,
                    Note = request.EmployeeExtraData.Note,

                    // ===== Organization =====
                    CompanyId = request.EmployeeOrganization.CompanyId,
                    DepartmentId = request.EmployeeOrganization.DepartmentId,
                    JobLevelId = request.EmployeeOrganization.JobLevelId,
                    JobTitleId = request.EmployeeOrganization.JobTitleId,
                    ManagerId = request.EmployeeOrganization.ManagerId,

                    // ===== Hiring =====
                    ContractTypeId = request.EmployeeOrganizationHiring.ContractTypeId,
                    SerialMobile = request.EmployeeOrganizationHiring.SerialMobile,
                    EmployeeCodeFinance = request.EmployeeOrganizationHiring.EmployeeCodeFinance,
                    EmployeeCodeHr = request.EmployeeOrganizationHiring.EmployeeCodeHr,
                    HireDate = request.EmployeeOrganizationHiring.HireDate,
                    StartDate = request.EmployeeOrganizationHiring.StartDate,
                    EndDate = request.EmployeeOrganizationHiring.EndDate,
                    Status = request.EmployeeOrganizationHiring.Status,

                    // ===== Work Rules =====
                    ShiftId = request.EmployeeShiftWorkDays.ShiftId,
                    WorkDaysId = request.EmployeeShiftWorkDays.WorkDaysId,

                    // ===== Audit =====
                    CreatedBy = userId,
                    CreatedAt = now,
                    UpdatedBy = userId,
                    UpdatedAt = now
                };

                _db.TbEmployees.Add(employee);
                await _db.SaveChangesAsync(cancellationToken);

                var employeeId = employee.EmployeeId;

                #endregion

                #region Work Locations

                foreach (var loc in request.EmployeeWorkLocations.Locations)
                {
                    _db.TbEmployeeWorkLocations.Add(new TbEmployeeWorkLocation
                    {
                        EmployeeId = employeeId,
                        CityId = loc.CityId,
                        WorkLocationId = loc.WorkLocationId,
                        CompanyId = loc.CompanyId,
                        CreatedBy = userId,
                        CreatedAt = now
                    });
                }

                #endregion

                #region Project (Single)

                if (request.EmployeeOrganization.ProjectId.HasValue)
                {
                    _db.TbEmployeeProjects.Add(new TbEmployeeProject
                    {
                        EmployeeId = employeeId,
                        ProjectId = request.EmployeeOrganization.ProjectId.Value,
                        CompanyId = request.EmployeeOrganization.CompanyId,
                        CreatedBy = userId,
                        CreatedAt = now
                    });
                }

                #endregion

                #region Vacation Balances

                foreach (var vb in request.EmployeeVacationsBalance.Balances)
                {
                    _db.TbEmployeeVacationBalances.Add(new TbEmployeeVacationBalance
                    {
                        EmployeeId = employeeId,
                        VacationTypeId = vb.VacationTypeId,
                        Year = vb.Year,
                        TotalDays = vb.TotalDays,
                        UsedDays = 0,
                        RemainingDays = vb.TotalDays
                    });
                }

                #endregion

                await _db.SaveChangesAsync(cancellationToken);
                await tx.CommitAsync(cancellationToken);

                return employeeId;
            }
            catch
            {
                await tx.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }

}
