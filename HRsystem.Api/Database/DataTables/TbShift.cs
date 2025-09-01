using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbShift
{
    public int ShiftId { get; set; }

    public string ShiftName { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public bool IsFlexible { get; set; }

    public TimeOnly? MinStartTime { get; set; }

    public TimeOnly? MaxStartTime { get; set; }

    public int GracePeriodMinutes { get; set; }

    public decimal? RequiredWorkingHours { get; set; }

    public string? Notes { get; set; }

    public int CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual ICollection<TbShiftAssignment> TbShiftAssignments { get; set; } = new List<TbShiftAssignment>();

    public virtual ICollection<TbShiftRule> TbShiftRules { get; set; } = new List<TbShiftRule>();
}
