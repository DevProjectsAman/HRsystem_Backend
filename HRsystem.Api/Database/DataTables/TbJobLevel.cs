using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbJobLevel
{
    public int JobLevelId { get; set; }

    public string? JobLevelDesc { get; set; }

    public string? JobLevelCode { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<TbJobTitle> TbJobTitles { get; set; } = new List<TbJobTitle>();
}
