using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Employee_Mission")]
public partial class TbEmployeeMission
{
    [Key]
    public long MissionId { get; set; }

    public long ActivityId { get; set; }

    public DateTime StartDatetime { get; set; }

    public DateTime EndDatetime { get; set; }

    [MaxLength(255)]
    public string? MissionLocation { get; set; }

    [MaxLength(200)]
    public string? MissionReason { get; set; }

    public virtual TbEmployeeActivity Activity { get; set; } = null!;
}
