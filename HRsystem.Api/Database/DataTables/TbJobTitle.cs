using HRsystem.Api.Shared.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;
[Table("Tb_Job_Title")]
public partial class TbJobTitle
{
    [Key]
    public int JobTitleId { get; set; }

    public int DepartmentId { get; set; }

    //[MaxLength(55)]
    public LocalizedData TitleName { get; set; } = null!;

    public int? JobLevelId { get; set; }

    public int CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual TbDepartment Department { get; set; } = null!;

    public virtual TbJobLevel? JobLevel { get; set; }

    public virtual ICollection<TbEmployee> TbEmployees { get; set; } = new List<TbEmployee>();

    public virtual ICollection<TbShiftRule> TbShiftRules { get; set; } = new List<TbShiftRule>();
}
