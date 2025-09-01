using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbCompany
{
    public int CompanyId { get; set; }

    public int GroupId { get; set; }

    public string CompanyName { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbGroup Group { get; set; } = null!;

    public virtual ICollection<TbActivityStatus> TbActivityStatuses { get; set; } = new List<TbActivityStatus>();

    public virtual ICollection<TbActivityType> TbActivityTypes { get; set; } = new List<TbActivityType>();

    public virtual ICollection<TbAttendanceLog> TbAttendanceLogs { get; set; } = new List<TbAttendanceLog>();

    public virtual ICollection<TbAuditLog> TbAuditLogs { get; set; } = new List<TbAuditLog>();

    public virtual ICollection<TbDepartment> TbDepartments { get; set; } = new List<TbDepartment>();

    public virtual ICollection<TbEmployeeActivity> TbEmployeeActivities { get; set; } = new List<TbEmployeeActivity>();

    public virtual ICollection<TbEmployeeProject> TbEmployeeProjects { get; set; } = new List<TbEmployeeProject>();

    public virtual ICollection<TbEmployeeWorkLocation> TbEmployeeWorkLocations { get; set; } = new List<TbEmployeeWorkLocation>();

    public virtual ICollection<TbEmployee> TbEmployees { get; set; } = new List<TbEmployee>();

    public virtual ICollection<TbJobTitle> TbJobTitles { get; set; } = new List<TbJobTitle>();

    public virtual ICollection<TbLeaveBalance> TbLeaveBalances { get; set; } = new List<TbLeaveBalance>();

    public virtual ICollection<TbLeaveRequest> TbLeaveRequests { get; set; } = new List<TbLeaveRequest>();

    public virtual ICollection<TbLeaveType> TbLeaveTypes { get; set; } = new List<TbLeaveType>();

    public virtual ICollection<TbProject> TbProjects { get; set; } = new List<TbProject>();

    public virtual ICollection<TbShiftAssignment> TbShiftAssignments { get; set; } = new List<TbShiftAssignment>();

    public virtual ICollection<TbShiftRule> TbShiftRules { get; set; } = new List<TbShiftRule>();

    public virtual ICollection<TbShift> TbShifts { get; set; } = new List<TbShift>();

    public virtual ICollection<TbWorkLocation> TbWorkLocations { get; set; } = new List<TbWorkLocation>();
}
