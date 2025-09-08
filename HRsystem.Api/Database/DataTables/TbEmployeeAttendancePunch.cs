using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Employee_Attendance_Punch")]
public partial class TbEmployeeAttendancePunch
{
    [Key]
    public long PunchId { get; set; }

    public long AttendanceId { get; set; }

    public DateTime PunchIn { get; set; }

    public DateTime? PunchOut { get; set; }

    public int? LocationId { get; set; }

    public int? DeviceId { get; set; }

    public virtual TbEmployeeAttendance Attendance { get; set; } = null!;
}
