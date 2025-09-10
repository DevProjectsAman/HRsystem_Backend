using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Company")]
public partial class TbCompany
{
    [Key]
    public int CompanyId { get; set; }

    public int GroupId { get; set; }
    [MaxLength(100)]
    public string CompanyName { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbGroup Group { get; set; } = null!;

    [MaxLength(400)]
    public string  CompanyLogo { get; set; } = null!;


    public virtual ICollection<TbActivityStatus> TbActivityStatuses { get; set; } = new List<TbActivityStatus>();

    public virtual ICollection<TbActivityType> TbActivityTypes { get; set; } = new List<TbActivityType>();

    public virtual ICollection<TbAuditLog> TbAuditLogs { get; set; } = new List<TbAuditLog>();

    public virtual ICollection<TbDepartment> TbDepartments { get; set; } = new List<TbDepartment>();

    public virtual ICollection<TbEmployeeProject> TbEmployeeProjects { get; set; } = new List<TbEmployeeProject>();

    public virtual ICollection<TbEmployeeShift> TbEmployeeShifts { get; set; } = new List<TbEmployeeShift>();

    public virtual ICollection<TbEmployeeWorkLocation> TbEmployeeWorkLocations { get; set; } = new List<TbEmployeeWorkLocation>();

    public virtual ICollection<TbEmployee> TbEmployees { get; set; } = new List<TbEmployee>();

    public virtual ICollection<TbJobTitle> TbJobTitles { get; set; } = new List<TbJobTitle>();

    public virtual ICollection<TbProject> TbProjects { get; set; } = new List<TbProject>();

    public virtual ICollection<TbShiftRule> TbShiftRules { get; set; } = new List<TbShiftRule>();

    public virtual ICollection<TbShift> TbShifts { get; set; } = new List<TbShift>();

    public virtual ICollection<TbWorkLocation> TbWorkLocations { get; set; } = new List<TbWorkLocation>();
}
