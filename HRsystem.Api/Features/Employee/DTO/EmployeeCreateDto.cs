using HRsystem.Api.Shared.DTO;
using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.Employee.DTO
{


    public class EmployeeCreateDto
    {
        public string EmployeeCodeFinance { get; set; }
        public string EmployeeCodeHr { get; set; }
        public string FirstName { get; set; }
        public string ArabicFirstName { get; set; }
        public string LastName { get; set; }
        public string ArabicLastName { get; set; }
        public DateOnly Birthdate { get; set; }
        public DateOnly HireDate { get; set; }
        public EnumGenderType Gender { get; set; }
        public string NationalId { get; set; }
        public string? PassportNumber { get; set; }
        public string PlaceOfBirth { get; set; }
        public string BloodGroup { get; set; }
        public int JobTitleId { get; set; }
        public int CompanyId { get; set; }
        public int DepartmentId { get; set; }
        public int ManagerId { get; set; }
        public int ShiftId { get; set; }
        public int WorkDaysId { get; set; }
        public int MaritalStatusId { get; set; }
        public int NationalityId { get; set; }
        public string Email { get; set; }
        public string PrivateMobile { get; set; }
        public string? BuisnessMobile { get; set; }
        public string SerialMobile { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public sbyte? IsTopManager { get; set; }
        public sbyte? IsFullDocument { get; set; }
        public string? Note { get; set; }
        public string Status { get; set; }

        // ✅ القوائم هتظهر دلوقتي في Swagger
        public List<EmployeeWorkLocationCreateDto> EmployeeWorkLocations { get; set; } = new();
        public List<EmployeeVacationBalanceCreateDto> EmployeeVacationBalances { get; set; } = new();
    }


    // Work Location DTO
    public record EmployeeWorkLocationCreateDto(
        int CityId,
        int WorkLocationId,
        int CompanyId
    );

    // Vacation Balance DTO
    public record EmployeeVacationBalanceCreateDto(
        int VacationTypeId,
        int Year,
        decimal TotalDays,
        decimal? UsedDays,
        decimal? RemainingDays
    );

    public class EmployeeUpdateDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeCodeFinance { get; set; }
        public string EmployeeCodeHr { get; set; }
        public string FirstName { get; set; }
        public string ArabicFirstName { get; set; }
        public string LastName { get; set; }
        public string ArabicLastName { get; set; }
        public DateOnly Birthdate { get; set; }
        public DateOnly HireDate { get; set; }
        public EnumGenderType Gender { get; set; }
        public string NationalId { get; set; }
        public string? PassportNumber { get; set; }
        public string PlaceOfBirth { get; set; }
        public string BloodGroup { get; set; }
        public int JobTitleId { get; set; }
        public int CompanyId { get; set; }
        public int DepartmentId { get; set; }
        public int ManagerId { get; set; }
        public int ShiftId { get; set; }
        public int MaritalStatusId { get; set; }
        public int NationalityId { get; set; }
        public string Email { get; set; }
        public string PrivateMobile { get; set; }
        public string? BuisnessMobile { get; set; }
        public string SerialMobile { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsTopManager { get; set; }
        public bool? IsFullDocument { get; set; }
        public string? Note { get; set; }
        public string Status { get; set; }
    };


    public class EmployeeReadDto
    {
        
        public int EmployeeId { get; set; }
        public string EmployeeCodeFinance { get; set; }
    public string EmployeeCodeHr { get; set; }
    public int JobTitleId { get; set; }
    public LocalizedData JobTitleName { get; set; }
    public string FirstName { get; set; }
    public string ArabicFirstName { get; set; }
    public string LastName { get; set; }
    public string ArabicLastName { get; set; }
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
