using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbAttendanceLog
{
    public int AttendanceId { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly AttendanceDate { get; set; }

    public TimeOnly? CheckIn { get; set; }

    public TimeOnly? CheckOut { get; set; }

    public int CompanyId { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual TbEmployee Employee { get; set; } = null!;
}
