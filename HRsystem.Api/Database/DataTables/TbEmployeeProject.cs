using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbEmployeeProject
{
    public int EmployeeProjectId { get; set; }

    public int EmployeeId { get; set; }

    public int ProjectId { get; set; }

    public int CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual TbEmployee Employee { get; set; } = null!;

    public virtual TbProject Project { get; set; } = null!;
}
