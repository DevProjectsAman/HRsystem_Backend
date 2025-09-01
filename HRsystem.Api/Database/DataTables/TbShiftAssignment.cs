using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbShiftAssignment
{
    public int AssignmentId { get; set; }

    public int EmployeeId { get; set; }

    public int ShiftId { get; set; }

    public DateOnly EffectiveDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Notes { get; set; }

    public int CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual TbEmployee Employee { get; set; } = null!;

    public virtual TbShift Shift { get; set; } = null!;
}
