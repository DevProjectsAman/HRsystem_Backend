using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Work_Location")]
public partial class TbWorkLocation
{
    [Key]
    public int WorkLocationId { get; set; }

    public int CompanyId { get; set; }

    [MaxLength(25)]
    public string? WorkLocationCode { get; set; }
    [MaxLength(100)]
    public string LocationName { get; set; } = null!;

    [Precision(9, 6)]
    public decimal? Latitude { get; set; }
    [Precision(9, 6)]
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
