using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbEmployee
{
    public int EmployeeId { get; set; }

    public string? EmployeeCodeFinance { get; set; }

    public string? EmployeeCodeHr { get; set; }

    public int JobTitleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? ArabicFirstName { get; set; }

    public string LastName { get; set; } = null!;

    public string? ArabicLastName { get; set; }

    public string? ArabicFullName { get; set; }

    public DateOnly? HireDate { get; set; }

    public DateOnly? Birthdate { get; set; }

    public string Gender { get; set; } = null!;

    public string? Nationality { get; set; }

    public string? NationalId { get; set; }

    public string? PassportNumber { get; set; }

    public string? MaritalStatus { get; set; }

    public string? PlaceOfBirth { get; set; }

    public string? BloodGroup { get; set; }

    public int? ManagerId { get; set; }

    public int CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual ICollection<TbEmployee> InverseManager { get; set; } = new List<TbEmployee>();

    public virtual TbJobTitle JobTitle { get; set; } = null!;

    public virtual TbEmployee? Manager { get; set; }

    public virtual ICollection<TbAttendanceLog> TbAttendanceLogs { get; set; } = new List<TbAttendanceLog>();

    public virtual ICollection<TbEmployeeActivity> TbEmployeeActivities { get; set; } = new List<TbEmployeeActivity>();

    public virtual ICollection<TbEmployeeProject> TbEmployeeProjects { get; set; } = new List<TbEmployeeProject>();

    public virtual ICollection<TbEmployeeWorkLocation> TbEmployeeWorkLocations { get; set; } = new List<TbEmployeeWorkLocation>();

    public virtual ICollection<TbLeaveBalance> TbLeaveBalances { get; set; } = new List<TbLeaveBalance>();

    public virtual ICollection<TbLeaveRequest> TbLeaveRequestApprovedByNavigations { get; set; } = new List<TbLeaveRequest>();

    public virtual ICollection<TbLeaveRequest> TbLeaveRequestEmployees { get; set; } = new List<TbLeaveRequest>();

    public virtual ICollection<TbShiftAssignment> TbShiftAssignments { get; set; } = new List<TbShiftAssignment>();
}
