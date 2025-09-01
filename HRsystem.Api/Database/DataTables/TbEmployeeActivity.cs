using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbEmployeeActivity
{
    public long ActivityId { get; set; }

    public int EmployeeId { get; set; }

    public int ActivityTypeId { get; set; }

    public int StatusId { get; set; }

    public DateOnly ActivityDate { get; set; }

    public DateTime? StartDatetime { get; set; }

    public DateTime? EndDatetime { get; set; }

    public TimeOnly? CheckIn { get; set; }

    public TimeOnly? CheckOut { get; set; }

    public int? RequestBy { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public int CompanyId { get; set; }

    public DateTime? RequestDate { get; set; }

    public virtual TbActivityType ActivityType { get; set; } = null!;

    public virtual TbCompany Company { get; set; } = null!;

    public virtual TbEmployee Employee { get; set; } = null!;

    public virtual TbActivityStatus Status { get; set; } = null!;
}
