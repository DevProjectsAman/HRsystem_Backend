using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.Employee.DTO
{
    public record CompleteEmployeeProfileDto(
      string? EmployeeCodeFinance,
      int JobTitleId,
      int DepartmentId,
      int ManagerId,
      int ShiftId,
      int MaritalStatusId,
      sbyte? IsTopManager,
      sbyte? IsFullDocument
  );
}
