using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Employee_Attendance")]
public partial class TbEmployeeAttendance
{
    [Key]
    public long AttendanceId { get; set; }

    public long ActivityId { get; set; }

    public DateTime AttendanceDate { get; set; }

    public DateTime? FirstPuchin { get; set; }

    public DateTime? LastPuchout { get; set; }
    [Precision(5, 2)]
    public decimal? TotalHours { get; set; }

    public virtual TbEmployeeActivity Activity { get; set; } = null!;

    public virtual ICollection<TbEmployeeAttendancePunch> TbEmployeeAttendancePunches { get; set; } = new List<TbEmployeeAttendancePunch>();
}
