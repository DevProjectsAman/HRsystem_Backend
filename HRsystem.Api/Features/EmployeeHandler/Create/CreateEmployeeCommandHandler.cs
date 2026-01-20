
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Features.EmployeeHandler.Create;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeHandler.Create
{



    public sealed class CreateEmployeeCommandHandler
        : IRequestHandler<CreateEmployeeCommandNew, int>
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
            CreateEmployeeCommandNew request,
            CancellationToken cancellationToken)
        {
            using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var now = DateTime.UtcNow;
                var userId = _currentUser.UserId;

                #region Create Employee (Aggregate Root)

                var empPhotoPath = _db.TbEmployeeCodeTrackings.Where(e => e.UniqueEmployeeCode == request.EmployeeBasicData.UniqueEmployeeCode && e.IsUsed == false)
                    .Select(e => e.DocFullPath).FirstOrDefault();

                string? photoPath = empPhotoPath != null ? empPhotoPath : null;

                var employee = new TbEmployee
                {
                    // ===== Basic Data =====
                    EnglishFullName = request.EmployeeBasicData.EnglishFullName,
                    ArabicFullName = request.EmployeeBasicData.ArabicFullName,
                    NationalId = request.EmployeeBasicData.NationalId,
                    Birthdate = request.EmployeeBasicData.Birthdate,
                    PlaceOfBirth = request.EmployeeBasicData.PlaceOfBirth ?? "Unknown",
                    Gender = request.EmployeeBasicData.Gender,
                    EmployeePhotoPath = photoPath,
                    UniqueEmployeeCode = request.EmployeeBasicData.UniqueEmployeeCode,

                    // ===== Extra Data =====
                    PassportNumber = request.EmployeeExtraData.PassportNumber,
                    MaritalStatusId = request.EmployeeExtraData.MaritalStatusId,
                    NationalityId = request.EmployeeExtraData.NationalityId,
                    PrivateMobile = request.EmployeeExtraData.PrivateMobile,
                    BuisnessMobile = request.EmployeeExtraData.BuisnessMobile,
                    Email = request.EmployeeExtraData.Email ?? $"{request.EmployeeBasicData.UniqueEmployeeCode}@Any.com" ,
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


                #region Employee Manager

                var managerId = request.EmployeeOrganization?.ManagerId;
                if (managerId is > 0)
                {
                    var empManager = await _db.TbEmployees
                        .FirstOrDefaultAsync(e => e.EmployeeId == managerId, cancellationToken);
                    if (empManager != null)
                    {
                        empManager.IsTopmanager = 1;
                        await _db.SaveChangesAsync(cancellationToken);
                    }
                }
                #endregion


                #region Employee Code Tracking Update
                var empCodeTrack = await _db.TbEmployeeCodeTrackings
                    .FirstOrDefaultAsync(e => e.UniqueEmployeeCode ==  request.EmployeeBasicData.UniqueEmployeeCode && e.IsUsed == false, cancellationToken);
                if (empCodeTrack != null)
                {
                    empCodeTrack.IsUsed = true;
                    empCodeTrack.UsedAt = now;
                    empCodeTrack.GeneratedById = userId;
                    _db.TbEmployeeCodeTrackings.Update(empCodeTrack);
                    await _db.SaveChangesAsync(cancellationToken);
                }

                    #endregion


                    #region Work Locations

                    foreach (var loc in request.EmployeeWorkLocations.EmployeeWorkLocations)
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

                #region Shift (Single)

                if (request.EmployeeShiftWorkDays.ShiftId > 0)
                {
                    _db.TbEmployeeShifts.Add(new TbEmployeeShift
                    {
                        EmployeeId = employeeId,
                        ShiftId = request.EmployeeShiftWorkDays.ShiftId,
                        CompanyId = request.EmployeeOrganization.CompanyId,
                        CreatedBy = userId,
                        CreatedAt = now,
                        EffectiveDate = DateOnly.FromDateTime(now)
                    });
                }

                #endregion


                #region Vacation Balances

                foreach (var vb in request.EmployeeVacationsBalance.EmployeeVacationBalances)
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
