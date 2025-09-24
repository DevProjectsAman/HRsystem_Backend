using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.Employee.DTO
{


public record EmployeeCreateDto(
        //string? EmployeeCodeFinance,
        string EmployeeCodeHr,
        string FirstName,
        string? ArabicFirstName,
        string LastName,
        string? ArabicLastName,
        DateOnly? Birthdate,
        DateOnly? HireDate,
        EnumGenderType Gender,
        string? NationalId,
        string? PassportNumber,
        string? PlaceOfBirth,
        string? BloodGroup,
       // int? JobTitleId,
        int CompanyId,
       // int? DepartmentId,
       // int? ManagerId,
       // int? ShiftId,
       // int? MaritalStatusId,
        int? NationalityId,
        string? Email,
        string? PrivateMobile,
        string? BuisnessMobile,
        string? SerialMobile,
        DateTime? StartDate,
        DateTime? EndDate,
       // sbyte? IsTopManager,
       // sbyte? IsFullDocument,
        string? Note,
        string? Status
    );

    
    public record EmployeeUpdateDto(
       int EmployeeId,
       string? EmployeeCodeFinance,
       string EmployeeCodeHr,
       string FirstName,
       string? ArabicFirstName,
       string LastName,
       string? ArabicLastName,
       DateOnly? Birthdate,
       DateOnly? HireDate,
       EnumGenderType Gender,
       string? NationalId,
       string? PassportNumber,
       string? PlaceOfBirth,
       string? BloodGroup,
       int JobTitleId,
       int CompanyId,
       int? DepartmentId,
       int? ManagerId,
       int? ShiftId,
       int? MaritalStatusId,
       int? NationalityId,
       string? Email,
       string? PrivateMobile,
       string? BuisnessMobile,
       string? SerialMobile,
       DateTime? StartDate,
       DateTime? EndDate,
       bool? IsTopManager,
       bool? IsFullDocument,
       string? Note,
       string? Status
   );


    public record EmployeeReadDto(
        int EmployeeId,
        string? EmployeeCodeFinance,
        string? EmployeeCodeHr,
        int JobTitleId,
        string JobTitleName,
        string FirstName,
        string? ArabicFirstName,
        string LastName,
        string? ArabicLastName,
        DateOnly? HireDate,
        DateOnly? Birthdate,
        EnumGenderType Gender,
        string? NationalId,
        string? PassportNumber,
        string? PlaceOfBirth,
        string? BloodGroup,
        int? ManagerId,
        string? ManagerName,
        int CompanyId,
        string CompanyName,
        int? CreatedBy,
        DateTime? CreatedAt,
        int? UpdatedBy,
        DateTime? UpdatedAt,
        string? PrivateMobile,
        string? BuisnessMobile,
        string? Email,
        string? SerialMobile,
        DateTime? StartDate,
        DateTime? EndDate,
        bool? IsTopManager,
        bool? IsFullDocument,
        string? Note,
        string? Status,
        int? NationalityId,
        string? NationalityName,
        int? DepartmentId,
        string? DepartmentName,
        int? ShiftId,
        string? ShiftName,
        int? MaritalStatusId,
        string? MaritalStatusName
    );

    public class NewEmployeeIdDTO
    {
        public int EmployeeId { get; set; }
    }
}
