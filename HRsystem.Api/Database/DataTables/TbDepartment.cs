using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Department")]
public partial class TbDepartment
{
    [Key]
    public int DepartmentId { get; set; }

    [MaxLength(25)]
    public string? DepartmentCode { get; set; }
    [MaxLength(55)]
    public string DepartmentName { get; set; } = null!;

    public int? CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCompany? Company { get; set; }

    public virtual ICollection<TbJobTitle> TbJobTitles { get; set; } = new List<TbJobTitle>();
}
