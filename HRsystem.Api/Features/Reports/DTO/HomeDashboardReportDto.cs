namespace HRsystem.Api.Features.Reports.DTO
{

    public class DepartmentAttendanceDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public double AttendancePercentage { get; set; } // %
        public double AbsencePercentage { get; set; } // %
    }

    public class HomeDashboardReportDto
    {
       
            public int TotalEmployees { get; set; }
            public int ActiveEmployees { get; set; }
            public int InactiveEmployees { get; set; }
            public int TotalDepartments { get; set; }
            public int TotalCompanies { get; set; }
            public int TotalRequests { get; set; }
            public int TotalApprovedRequests { get; set; }
            public int TotalPendingRequests { get; set; }

            public List<DepartmentAttendanceDto> TodayDepartmentStatus { get; set; }

    }
}
