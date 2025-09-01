using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbEmployeeWorkLocation
{
    public int EmployeeWorkLocationId { get; set; }

    public int EmployeeId { get; set; }

    public int CityId { get; set; }

    public int WorkLocationId { get; set; }

    public int CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual TbEmployee Employee { get; set; } = null!;

    public virtual TbWorkLocation WorkLocation { get; set; } = null!;
}
