namespace HRsystem.Api.Features.Reports.DTO
{
    public class EmployeeActivityReportByManagerDto
    {
        public List<EmployeeActivityRowDto> Rows { get; set; } = new();
        public List<ActivitySummaryDto> Summary { get; set; } = new();
    }

    public class EmployeeActivityRowDto
    {
        public int? DayId { get; set; }
        public DateTime Date { get; set; }
        public int? EmployeeId { get; set; }
        public string EnglishFullName { get; set; }
        public string ArabicFullName { get; set; }
        public int? ContractTypeId { get; set; }
        public string EmployeeCodeFinance { get; set; }
        public string EmployeeCodeHr { get; set; }
        public int? JobTitleId { get; set; }
        public int? JobLevelId { get; set; }
        public int? ManagerId { get; set; }
        public int? CompanyId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ShiftId { get; set; }
        public int? WorkDaysId { get; set; }
        public int? RemoteWorkDaysId { get; set; }
        public long? ActivityId { get; set; }
        public int? ActivityTypeId { get; set; }
        public int? EmployeeTodayStatuesId { get; set; }
        public long? RequestBy { get; set; }
        public long? ApprovedBy { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public long? AttendanceId { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public DateTime? FirstPuchin { get; set; }
        public int? AttStatues { get; set; }
        public DateTime? LastPuchout { get; set; }
        public decimal? TotalHours { get; set; }
        public decimal? ActualWorkingHours { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsWorkday { get; set; }
        public bool IsRemoteday { get; set; }
        public string TodayStatues { get; set; }
        public string Details { get; set; }


        // ✅ new names
        public string JobTitleName { get; set; }
        public string JobLevelCode { get; set; }
        public string ManagerName { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string ShiftName { get; set; }
        public string ShiftStartTime { get; set; }
        public string ShiftEndTime { get; set; }
    }

    public class ActivitySummaryDto
    {
        public string ActivityTypeCode { get; set; }
        public string ActivityTypeName { get; set; }
        public int Count { get; set; }
    }

}



