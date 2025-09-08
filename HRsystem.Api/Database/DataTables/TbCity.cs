using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_City")]
public partial class TbCity
{
    [Key]
    public int CityId { get; set; }

    public int? GovId { get; set; }

    public string? CityName { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbGov? Gov { get; set; }

    public virtual ICollection<TbProject> TbProjects { get; set; } = new List<TbProject>();

    public virtual ICollection<TbWorkLocation> TbWorkLocations { get; set; } = new List<TbWorkLocation>();
}
