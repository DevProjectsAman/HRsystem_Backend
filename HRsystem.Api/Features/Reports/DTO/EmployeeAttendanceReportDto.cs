using System.Collections.Generic;

namespace HRsystem.Api.Features.Reports.DTO
{
    public class EmployeeAttendanceStatusDto
    {
        public string StatusCode { get; set; }   // "attendance", "absent", "vacation"...
        public string StatusName { get; set; }
        public double Percentage { get; set; }
    }

    public class EmployeeAttendanceReportDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public List<EmployeeAttendanceStatusDto> Statuses { get; set; }
    }
}
