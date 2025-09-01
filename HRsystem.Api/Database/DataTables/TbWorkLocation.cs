using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbWorkLocation
{
    public int WorkLocationId { get; set; }

    public int CompanyId { get; set; }

    public string LocationName { get; set; } = null!;

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public int? AllowedRadiusM { get; set; }

    public int? CityId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCity? City { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual ICollection<TbEmployeeWorkLocation> TbEmployeeWorkLocations { get; set; } = new List<TbEmployeeWorkLocation>();

    public virtual ICollection<TbShiftRule> TbShiftRules { get; set; } = new List<TbShiftRule>();
}
