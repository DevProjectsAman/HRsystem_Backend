using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.EmployeeEdit
{
    public class EmployeeFullDetailsDto
    {
        public int EmployeeId { get; set; }
        public EmployeeBasicDataDto BasicData { get; set; }
        public EmployeeExtraDataDto ExtraData { get; set; }
        public EmployeeOrganizationDto Organization { get; set; }
        public EmployeeHiringDto Hiring { get; set; }
        public List<EmployeeWorkLocationDto> WorkLocations { get; set; } = new();
        public List<EmployeeProjectDto> Projects { get; set; } = new();
        public EmployeeShiftWorkDaysDto ShiftWorkDays { get; set; }
        public List<EmployeeVacationBalanceDto> VacationBalances { get; set; } = new();
    }

    #region Section DTOs

    public class EmployeeBasicDataDto
    {
        public string EnglishFullName { get; set; }
        public string ArabicFullName { get; set; }
        public string NationalId { get; set; }
        public DateOnly Birthdate { get; set; }
        public string? PlaceOfBirth { get; set; }
        public EnumGenderType Gender { get; set; }
        public string? EmployeePhotoPath { get; set; }
        public string UniqueEmployeeCode { get; set; }
    }

    public class EmployeeExtraDataDto
    {
        public string? PassportNumber { get; set; }
        public int MaritalStatusId { get; set; }
        public string? MaritalStatusName { get; set; }
        public int NationalityId { get; set; }
        public string? NationalityName { get; set; }
        public string? Email { get; set; }
        public string PrivateMobile { get; set; }
        public string? BuisnessMobile { get; set; }
        public string? Address { get; set; }
        public EnumReligionType Religion { get; set; }
        public string? BloodGroup { get; set; }
        public string? Note { get; set; }
    }

    public class EmployeeOrganizationDto
    {
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? JobLevelId { get; set; }
        public string? JobLevelName { get; set; }
        public int JobTitleId { get; set; }
        public string? JobTitleName { get; set; }
        public int ManagerId { get; set; }
        public string? ManagerName { get; set; }

        public int? ProjectId { get; set; }
    }

    public class EmployeeHiringDto
    {
        public int ContractTypeId { get; set; }
        public string? SerialMobile { get; set; }
        public string? EmployeeCodeFinance { get; set; }
        public string? EmployeeCodeHr { get; set; }
        public DateOnly HireDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }

    public class EmployeeWorkLocationDto
    {
        public int EmployeeWorkLocationId { get; set; }
        public int CityId { get; set; }
        public string? CityName { get; set; }
        public int WorkLocationId { get; set; }
        public string? WorkLocationName { get; set; }
        public int CompanyId { get; set; }
    }

    public class EmployeeProjectDto
    {
        public int EmployeeProjectId { get; set; }
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public int CompanyId { get; set; }
    }

    public class EmployeeShiftWorkDaysDto
    {
        public int ShiftId { get; set; }
        public string? ShiftName { get; set; }
        public int WorkDaysId { get; set; }
        public string? WorkDaysName { get; set; }
        public int? RemoteWorkDaysId { get; set; }
        public string? RemoteWorkDaysName { get; set; }
    }

    public class EmployeeVacationBalanceDto
    {
        public int BalanceId { get; set; }
        public int VacationTypeId { get; set; }
        public string? VacationTypeName { get; set; }
        public int Year { get; set; }
        public decimal TotalDays { get; set; }
        public decimal? UsedDays { get; set; }
        public decimal? RemainingDays { get; set; }
    }

    #endregion
}