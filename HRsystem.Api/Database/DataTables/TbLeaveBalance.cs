using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbLeaveBalance
{
    public int BalanceId { get; set; }

    public int EmployeeId { get; set; }

    public int LeaveTypeId { get; set; }

    public decimal? BalanceDays { get; set; }

    public decimal? BalanceRemaining { get; set; }

    public int CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual TbEmployee Employee { get; set; } = null!;

    public virtual TbLeaveType LeaveType { get; set; } = null!;
}
