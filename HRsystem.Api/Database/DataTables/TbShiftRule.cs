using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbShiftRule
{
    public int RuleId { get; set; }

    public int? JobTitleId { get; set; }

    public int? WorkingLocationId { get; set; }

    public int? ProjectId { get; set; }

    public int ShiftId { get; set; }

    public int? Priority { get; set; }

    public int CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual TbJobTitle? JobTitle { get; set; }

    public virtual TbProject? Project { get; set; }

    public virtual TbShift Shift { get; set; } = null!;

    public virtual TbWorkLocation? WorkingLocation { get; set; }
}
