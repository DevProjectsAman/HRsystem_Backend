using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbLeaveRequest
{
    public int RequestId { get; set; }

    public int EmployeeId { get; set; }

    public int LeaveTypeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string? Status { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public int? RequestBy { get; set; }

    public DateTime? RequestDate { get; set; }

    public int CompanyId { get; set; }

    public virtual TbEmployee? ApprovedByNavigation { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual TbEmployee Employee { get; set; } = null!;

    public virtual TbLeaveType LeaveType { get; set; } = null!;
}
