using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.Employee.DTO
{
         public record EmployeeCreateDto(
        string EmployeeCodeHr,
        string FirstName,
        string LastName,
        DateOnly? Birthdate,
        DateOnly? HireDate,
        string? NationalId,
        string? PassportNumber,
        int JobTitleId,
        int CompanyId,
        int? DepartmentId,
        int? ManagerId,
        string? Email,
        string? PrivateMobile,
        string? BuisnessMobile,
        EnumGenderType Gender
    );



    public record EmployeeUpdateDto(
        int EmployeeId,
        string EmployeeCodeHr,
        string FirstName,
        string LastName,
        DateOnly? Birthdate,
        DateOnly? HireDate,
        string? NationalId,
        string? PassportNumber,
        int JobTitleId,
        int CompanyId,
        int? DepartmentId,
        int? ManagerId,
        string? Email,
        string? PrivateMobile,
        string? BuisnessMobile,
        EnumGenderType Gender,
        string? Status
    );

    public record EmployeeReadDto(
        int EmployeeId,
        string EmployeeCodeHr,
        string FirstName,
        string LastName,
        string? Email,
        string? PrivateMobile,
        string? BuisnessMobile,
        EnumGenderType Gender,
        string JobTitleName,
        string CompanyName,
        string? DepartmentName,
        string? ManagerName,
        DateOnly? HireDate,
        DateOnly? Birthdate,
        string? Status
    );


    public class NewEmployeeIdDTO
    {
        public int EmployeeId { get; set; }
    }
}
