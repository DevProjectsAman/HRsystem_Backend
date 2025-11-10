using HRsystem.Api.Shared.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;



[Table("Tb_Attendance_Statues")]
public partial class TbAttendanceStatues
{
    [Key]
    public int AttendanceStatuesId { get; set; }

    [MaxLength(25)]
    public string AttendanceStatuesCode { get; set; } = null!;

    //[Column(TypeName = "json")]
    public LocalizedData AttendanceStatuesName { get; set; }

}
