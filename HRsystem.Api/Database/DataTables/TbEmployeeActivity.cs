using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Employee_Activity")]
public partial class TbEmployeeActivity
{
    [Key]
    public long ActivityId { get; set; }

    public int EmployeeId { get; set; }

    public int ActivityTypeId { get; set; }

    public int StatusId { get; set; }

    public long RequestBy { get; set; }

    public long? ApprovedBy { get; set; }

    public DateTime RequestDate { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public long CompanyId { get; set; }

    public virtual TbActivityType ActivityType { get; set; } = null!;

    public virtual TbEmployee Employee { get; set; } = null!;

    public virtual TbActivityStatus Status { get; set; } = null!;

    public virtual ICollection<TbEmployeeActivityApproval> TbEmployeeActivityApprovals { get; set; } = new List<TbEmployeeActivityApproval>();

    public virtual ICollection<TbEmployeeAttendance> TbEmployeeAttendances { get; set; } = new List<TbEmployeeAttendance>();

    public virtual ICollection<TbEmployeeExcuse> TbEmployeeExcuses { get; set; } = new List<TbEmployeeExcuse>();

    public virtual ICollection<TbEmployeeMission> TbEmployeeMissions { get; set; } = new List<TbEmployeeMission>();

    public virtual ICollection<TbEmployeeVacation> TbEmployeeVacations { get; set; } = new List<TbEmployeeVacation>();
}
