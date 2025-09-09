using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Employee_Excuse")]
public partial class TbEmployeeExcuse
{
    [Key]
    public long ExcuseId { get; set; }

    public long ActivityId { get; set; }

    public DateOnly ExcuseDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    [MaxLength(150)]
    public string? ExcuseReason { get; set; }

    public virtual TbEmployeeActivity Activity { get; set; } = null!;
}
