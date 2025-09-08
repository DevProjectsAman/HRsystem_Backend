using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Activity_Status")]
public partial class TbActivityStatus
{
    [Key]
    public int StatusId { get; set; }

    public string StatusCode { get; set; } = null!;

    public string StatusName { get; set; } = null!;

    public bool IsFinal { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? CompanyId { get; set; }

    public virtual TbCompany? Company { get; set; }

    public virtual ICollection<TbEmployeeActivity> TbEmployeeActivities { get; set; } = new List<TbEmployeeActivity>();

    public virtual ICollection<TbEmployeeActivityApproval> TbEmployeeActivityApprovals { get; set; } = new List<TbEmployeeActivityApproval>();
}
