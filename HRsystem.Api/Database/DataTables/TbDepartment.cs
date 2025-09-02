using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbDepartment
{
    public int DepartmentId { get; set; }

    public string? DepartmentCode { get; set; }

    public string DepartmentName { get; set; } = null!;

    public int? CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCompany? Company { get; set; }

    public virtual ICollection<TbJobTitle> TbJobTitles { get; set; } = new List<TbJobTitle>();
}
