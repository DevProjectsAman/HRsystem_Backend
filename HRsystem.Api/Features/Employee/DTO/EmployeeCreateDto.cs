using HRsystem.Api.Shared.DTO;
using System.ComponentModel.DataAnnotations;
using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.Employee.DTO
{


    // Root DTO
    public class EmployeeCreateDto
    {
        [Required]
        public EmployeeBasicDataDto EmployeeBasicData { get; set; } = new();

        [Required]
        public EmployeeOrganizationDto EmployeeOrganization { get; set; } = new();

        [Required]
        public EmployeeWorkConditionsDto EmployeeWorkConditions { get; set; } = new();

        public List<EmployeeVacationBalanceCreateDto> EmployeeVacationBalances { get; set; } = new();
    }

    // ✅ Basic personal data
    public class EmployeeBasicDataDto
    {
        [Required, MaxLength(100)]
        public string EnglishFullName { get; set; }

        [Required, MaxLength(100)]
        public string ArabicFullName { get; set; }

        [Required, MaxLength(14)]
        public string NationalId { get; set; }

        [Required]
        public DateOnly Birthdate { get; set; }

        public string? PlaceOfBirth { get; set; }

        [Required]
        public EnumGenderType Gender { get; set; }

        public string? PassportNumber { get; set; }

        [Required]
        public int MaritalStatusId { get; set; }

        [Required]
        public int NationalityId { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PrivateMobile { get; set; }

        public string? BuisnessMobile { get; set; }

        public string? Address { get; set; }

        public string? EmployeePhotoPath { get; set; }

        public string? Note { get; set; }
    }

    // ✅ Organization data
    public class EmployeeOrganizationDto
    {
        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int JobTitleId { get; set; }

        public int ManagerId { get; set; }
        public int ContractTypeId { get; set; }

        [Required, MaxLength(25)]
        public string SerialMobile { get; set; }
        // public int? ProjectId { get; set; }

        public string? EmployeeCodeFinance { get; set; }
        public string? EmployeeCodeHr { get; set; }

        [Required]
        public DateOnly HireDate { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required, MaxLength(25)]
        public string Status { get; set; } = "Active";
    }

    // ✅ Work conditions
    public class EmployeeWorkConditionsDto
    {
        [Required]
        public int ShiftId { get; set; }

        [Required]
        public int WorkDaysId { get; set; }

        public List<EmployeeWorkLocationCreateDto> EmployeeWorkLocations { get; set; } = new();
    }

    // ✅ Work location
    public class EmployeeWorkLocationCreateDto
    {
        [Required]
        public int CityId { get; set; }

        [Required]
        public int WorkLocationId { get; set; }

        [Required]
        public int CompanyId { get; set; }
    }

    // ✅ Vacation balances
    public class EmployeeVacationBalanceCreateDto
    {
        [Required]
        public int VacationTypeId { get; set; }

        [Required]
        public int Year { get; set; }

        [Required, Range(0, 365)]
        public decimal TotalDays { get; set; }

        [Range(0, 365)]
        public decimal? UsedDays { get; set; }

        [Range(0, 365)]
        public decimal? RemainingDays { get; set; }
    }

    public class EmployeeUpdateDto
    {
        [Required]
        public int EmployeeId { get; set; }

        public EmployeeBasicDataUpdateDto? EmployeeBasicData { get; set; }

        public EmployeeOrganizationUpdateDto? EmployeeOrganization { get; set; }

        public EmployeeWorkConditionsUpdateDto? EmployeeWorkConditions { get; set; }

        public List<EmployeeVacationBalanceUpdateDto>? EmployeeVacationBalances { get; set; }
    }

    // ✅ Basic data update
    public class EmployeeBasicDataUpdateDto
    {
        [MaxLength(100)]
        public string? EnglishFullName { get; set; }

        [MaxLength(100)]
        public string? ArabicFullName { get; set; }

        [MaxLength(14)]
        public string? NationalId { get; set; }

        public DateOnly? Birthdate { get; set; }

        public string? PlaceOfBirth { get; set; }

        public EnumGenderType? Gender { get; set; }

        public string? PassportNumber { get; set; }

        public int? MaritalStatusId { get; set; }

        public int? NationalityId { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PrivateMobile { get; set; }

        public string? BuisnessMobile { get; set; }

        public string? Address { get; set; }

        public string? EmployeePhotoPath { get; set; }

        public string? Note { get; set; }
    }

    // ✅ Organization update
    public class EmployeeOrganizationUpdateDto
    {
        public int? CompanyId { get; set; }
        public int? DepartmentId { get; set; }
        public int? JobTitleId { get; set; }
        public int? ManagerId { get; set; }
        public int? ContractTypeId { get; set; }

        [MaxLength(25)]
        public string? SerialMobile { get; set; }

        public string? EmployeeCodeFinance { get; set; }
        public string? EmployeeCodeHr { get; set; }

        public DateOnly? HireDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [MaxLength(25)]
        public string? Status { get; set; }
    }

    // ✅ Work conditions update
    public class EmployeeWorkConditionsUpdateDto
    {
        public int? ShiftId { get; set; }
        public int? WorkDaysId { get; set; }
        public List<EmployeeWorkLocationUpdateDto>? EmployeeWorkLocations { get; set; }
    }

    // ✅ Work location update
    public class EmployeeWorkLocationUpdateDto
    {
        public int? CityId { get; set; }
        public int? WorkLocationId { get; set; }
        public int? CompanyId { get; set; }
    }

    // ✅ Vacation balance update
    public class EmployeeVacationBalanceUpdateDto
    {
        [Required]
        public int VacationTypeId { get; set; } // key field

        public int? Year { get; set; }

        [Range(0, 365)]
        public decimal? TotalDays { get; set; }

        [Range(0, 365)]
        public decimal? UsedDays { get; set; }

        [Range(0, 365)]
        public decimal? RemainingDays { get; set; }
    }

    public class EmployeeReadDto
    {

        public int EmployeeId { get; set; }
        public string EmployeeCodeFinance { get; set; }
        public string EmployeeCodeHr { get; set; }
        public int JobTitleId { get; set; }
        public LocalizedData JobTitleName { get; set; }
        // test
        public string? EnglishFullName { get; set;}
        public string? ArabicFullName { get; set; }
        public DateOnly HireDate { get; set; }
        public DateOnly Birthdate { get; set; }
        public EnumGenderType Gender { get; set; }
        public string NationalId { get; set; }
        public string? PassportNumber { get; set; }
        public string PlaceOfBirth { get; set; }
        public string BloodGroup { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string PrivateMobile { get; set; }
        public string? BuisnessMobile { get; set; }
        public string Email { get; set; }
        public string SerialMobile { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsTopManager { get; set; }
        public bool? IsFullDocument { get; set; }
        public string? Note { get; set; }
        public string Status { get; set; }
        public int NationalityId { get; set; }
        public string NationalityName { get; set; }
        public int DepartmentId { get; set; }
        public LocalizedData DepartmentName { get; set; }
        public int ShiftId { get; set; }
        public LocalizedData ShiftName { get; set; }
        public int MaritalStatusId { get; set; }
        public string MaritalStatusName { get; set; }
    }

    public class NewEmployeeIdDTO
    {
        public int EmployeeId { get; set; }
    }
}
