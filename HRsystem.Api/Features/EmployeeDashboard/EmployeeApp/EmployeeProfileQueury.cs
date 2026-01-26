using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
    public record EmployeeProfileQueury() : IRequest<EmployeeProfileQueuryDtoResponse>;

    public class EmployeeProfileQueuryHandler : IRequestHandler<EmployeeProfileQueury, EmployeeProfileQueuryDtoResponse>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentuser;

        public EmployeeProfileQueuryHandler( DBContextHRsystem db ,  ICurrentUserService currentuser)
        {
            _db = db;
            _currentuser = currentuser;
        }
        public async Task<EmployeeProfileQueuryDtoResponse> Handle(EmployeeProfileQueury request, CancellationToken ct)
        {

            var employeeId = _currentuser.EmployeeID;
            var language = _currentuser.UserLanguage;

            //var Employee = await _db.TbEmployees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            //var natiionality = await _db.TbNationalities.FirstOrDefaultAsync(n => n.NationalityId == Employee.NationalityId);
            //var vacation = await _db.TbEmployeeVacationBalances
            //    .Where(v => v.EmployeeId == employeeId)
            //    .ToListAsync(ct);
            //var departement = await _db.TbDepartments.FirstOrDefaultAsync(d => d.DepartmentId == Employee.DepartmentId, ct);
            //var workloc = await _db.TbEmployeeWorkLocations.FirstOrDefaultAsync(w => w.EmployeeId == Employee.EmployeeId, ct);
            //var manager = await _db.TbEmployees.FirstOrDefaultAsync(m => m.EmployeeId == Employee.ManagerId, ct);
            //var joblevel = await _db.TbJobLevels.FirstOrDefaultAsync(j => j.JobLevelId == Employee.JobLevelId, ct);
            //var shift = await _db.TbShifts.FirstOrDefaultAsync(s => s.ShiftId == Employee.ShiftId, ct);
            //var workdays = await _db.TbWorkDays.FirstOrDefaultAsync(wd => wd.WorkDaysId == Employee.WorkDaysId, ct);
            //var worklocation = await _db.TbWorkLocations.FirstOrDefaultAsync(wl => wl.WorkLocationId == workloc.WorkLocationId, ct);

            var employee = await _db.TbEmployees
                                    .Include(e => e.Nationality)
                                    .Include(e => e.Department)
                                    .Include(e => e.JobLevel)
                                    .Include (e => e.JobTitle)
                                    .Include(e => e.Shifts)
                                    .Include(e => e.TbWorkDays)
                                    .Include(e => e.Manager)
                                    .Include(e => e.TbEmployeeWorkLocations)
                                        .ThenInclude(w => w.WorkLocation)
                                    .Include(e => e.TbEmployeeVacationBalances)
                                    .FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);

            var nationality = employee.Nationality;
            var vacation = employee.TbEmployeeVacationBalances;
            var department = employee.Department;
            var workLocRelation = employee.TbEmployeeWorkLocations.FirstOrDefault();
            var manager = employee.Manager;
            var jobLevel = employee.JobLevel;
            var jobtittle = employee.JobTitle;
            var shift = employee.Shifts;
            var workDays = employee.TbWorkDays;
            var workLocation = workLocRelation?.WorkLocation;

            if (employee == null)
            {
                throw new Exception($"Employee Not Found ID= {employeeId}");

            }

            var infoScreen = new EmployeeInfoScreenDto
            {
                EnglishFullName = employee.EnglishFullName,
                ArabicFullName = employee.ArabicFullName,
                EmployeeCodeHr = employee.EmployeeCodeHr,
                StartDate = employee.StartDate,
                EmployeePhotoPath = employee.EmployeePhotoPath,
                JobTittle = language == "ar"
                ? jobtittle.TitleName.ar
                : jobtittle.TitleName.en
            };

            var personalInformation = new EmployeePresonalInformation
            {
                NationalId = employee.NationalId,
                Birthdate = employee.Birthdate,
                Nationality =language == "ar"
                ? nationality.NameAr
                : nationality.NameEn
            };

            var assistInfo = new EmployeeAssistInfoDto
            {
                SerialLaptop = employee.SerialMobile,
                SerialMobile = employee.SerialMobile
            };

            var vacationBalance = new EmployeeVacationBalanceDto
            {
                AnnualRemainBalance = vacation.FirstOrDefault(v => v.VacationTypeId == 1)?.RemainingDays,
                AnnualUsedBalance = vacation.FirstOrDefault(v => v.VacationTypeId == 1)?.UsedDays,
                CasualRemainBalance = vacation.FirstOrDefault(v => v.VacationTypeId == 5)?.RemainingDays,
                CasualUsedBalance = vacation.FirstOrDefault(v => v.VacationTypeId == 5)?.UsedDays,
                SickRemainBalance = vacation.FirstOrDefault(v => v.VacationTypeId == 2)?.RemainingDays,
                SickUsedBalance = vacation.FirstOrDefault(v => v.VacationTypeId == 2)?.UsedDays,
                UnPaidRemainBalance = vacation.FirstOrDefault(v => v.VacationTypeId == 3)?.RemainingDays,
                UnPaidUsedBalance = vacation.FirstOrDefault(v => v.VacationTypeId == 3)?.UsedDays,
                EmergencyRemainBalance = vacation.FirstOrDefault(v => v.VacationTypeId == 4)?.RemainingDays,
                EmergencyUsedBalance = vacation.FirstOrDefault(v => v.VacationTypeId == 4)?.UsedDays,
            };
      
            var organizationStructure = new EmployeeOrganizationStructureDto
            {
                Department = language == "ar"
                ? department.DepartmentName.ar
                : department.DepartmentName.en,
                Manager = manager != null
                ? (language == "ar" ? manager.ArabicFullName : manager.EnglishFullName)
                : null,
                JobLevel = jobLevel.JobLevelDesc ,
            };

            var work_Location = new EmployeeWork_LocationDto
            {
                Shift = language == "ar" ?shift.ShiftName.ar: shift.ShiftName.en ,
                WorkDays = workDays.WorkDaysDescription,
                WorkLocation = language == "ar"? workLocation.LocationName.ar : workLocation.LocationName.en,
            };

            return new EmployeeProfileQueuryDtoResponse
            {
                InfoScreen = infoScreen,
                VacationBalance = vacationBalance,
                PresonalInformation = personalInformation,
                OrganizationStructure = organizationStructure,
                Work_Location = work_Location,
                AssistInfo = assistInfo
            };
        }
    }

}

public class EmployeeProfileQueuryDtoResponse
{
    public EmployeeInfoScreenDto InfoScreen { get; set; }
    public EmployeeVacationBalanceDto VacationBalance { get; set; }

    public EmployeePresonalInformation PresonalInformation { get; set; }
    public EmployeeOrganizationStructureDto OrganizationStructure { get; set; }

    public EmployeeWork_LocationDto Work_Location { get; set; }

    public EmployeeAssistInfoDto AssistInfo { get; set; }

};

public class EmployeeInfoScreenDto
{
    [MaxLength(200)]
    public string EnglishFullName { get; set; }   // الاسم بالإنجليزي

    [MaxLength(200)]
    public string ArabicFullName { get; set; }    // الاسم بالعربي

    [MaxLength(55)]
    public string EmployeeCodeHr { get; set; }

    public DateTime StartDate { get; set; }

    [MaxLength(500)]
    public string? EmployeePhotoPath { get; set; } // صورة الموظف (path أو url)

    public string JobTittle { get; set; }
    

};
public class EmployeePresonalInformation
{
    public string NationalId { get; set; }
    public DateOnly Birthdate { get; set; }
    public string Nationality { get; set; }
};
public class EmployeeVacationBalanceDto
{
    public decimal? AnnualUsedBalance { get; set; }
    public decimal? AnnualRemainBalance { get; set; }

    public decimal? CasualUsedBalance { get; set; }
    public decimal? CasualRemainBalance { get; set; }

    public decimal? EmergencyUsedBalance { get; set; }
    public decimal? EmergencyRemainBalance { get; set; }

    public decimal? SickUsedBalance { get; set; }
    public decimal? SickRemainBalance { get; set; }

    public decimal? UnPaidUsedBalance { get; set; }
    public decimal? UnPaidRemainBalance { get; set; }


};
public class EmployeeOrganizationStructureDto
{
    public string Department { get; set; }
    public string Manager { get; set; }
    public string? JobLevel { get; set; }

};
public class EmployeeWork_LocationDto
{
    public string Shift { get; set; }

    public string WorkDays { get; set; }

    public string WorkLocation { get; set; }

};
public class EmployeeAssistInfoDto
{
    [MaxLength(25)]
    public string SerialMobile { get; set; }

    [MaxLength(25)]
    public string SerialLaptop { get; set; }

};
