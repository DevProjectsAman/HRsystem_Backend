using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbProject
{
    public int ProjectId { get; set; }

    public string? ProjectCode { get; set; }

    public string ProjectName { get; set; } = null!;

    public int? CityId { get; set; }

    public string? WorkLocationId { get; set; }

    public int CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCity? City { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual ICollection<TbEmployeeProject> TbEmployeeProjects { get; set; } = new List<TbEmployeeProject>();

    public virtual ICollection<TbShiftRule> TbShiftRules { get; set; } = new List<TbShiftRule>();
}
