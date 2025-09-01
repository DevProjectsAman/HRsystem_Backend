using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbLeaveType
{
    public int LeaveTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public int CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual ICollection<TbLeaveBalance> TbLeaveBalances { get; set; } = new List<TbLeaveBalance>();

    public virtual ICollection<TbLeaveRequest> TbLeaveRequests { get; set; } = new List<TbLeaveRequest>();
}
