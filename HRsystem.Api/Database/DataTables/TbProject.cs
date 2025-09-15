using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Project")]
public partial class TbProject
{
    [Key]
    public int ProjectId { get; set; }

    [MaxLength(25)]
    public string? ProjectCode { get; set; }
    //[MaxLength(100)]
    public string ProjectName { get; set; } = null!;

    public int? CityId { get; set; }

    public int? WorkLocationId { get; set; }

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
