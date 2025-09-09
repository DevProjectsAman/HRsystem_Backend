using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Employee_Activity_Approval")]
public partial class TbEmployeeActivityApproval
{
    [Key]
    public long ApprovalId { get; set; }

    public long ActivityId { get; set; }

    public int StatusId { get; set; }

    public int ChangedBy { get; set; }

    public DateTime ChangedDate { get; set; }

    [MaxLength(100)]
    public string? Notes { get; set; }

    public virtual TbEmployeeActivity Activity { get; set; } = null!;

    public virtual TbActivityStatus Status { get; set; } = null!;
}
