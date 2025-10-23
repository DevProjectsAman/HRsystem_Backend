namespace HRsystem.Api.Features.Reports.DTO
{
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
        
    }
}
