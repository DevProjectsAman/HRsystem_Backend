namespace HRsystem.Api.Features.Employee.DTO
{
   
   public record EmployeeWorkScheduleDto(
       string? EmployeeCodeFinance,
       int? ShiftId,
       int? RemoteWorkDaysId,
       int? WorkDaysId
      
   );
    
}
