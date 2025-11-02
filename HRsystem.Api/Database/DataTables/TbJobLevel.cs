using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;
[Table("Tb_Job_Level")]
[Index(nameof(JobLevelCode), nameof(CompanyId), IsUnique = true)]
[Index(nameof(JobLevelDesc), nameof(CompanyId), IsUnique = true)]

public partial class TbJobLevel
{
    [Key]
    public int JobLevelId { get; set; }

    [MaxLength(55)]
    public string? JobLevelDesc { get; set; }
    [MaxLength(25)]
    public string? JobLevelCode { get; set; }

    public int? CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<TbJobTitle> TbJobTitles { get; set; } = new List<TbJobTitle>();

}
