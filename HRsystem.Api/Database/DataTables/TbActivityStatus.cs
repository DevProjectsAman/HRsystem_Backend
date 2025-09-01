using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbActivityStatus
{
    public int StatusId { get; set; }

    public string StatusCode { get; set; } = null!;

    public string StatusName { get; set; } = null!;

    public bool IsFinal { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? CompanyId { get; set; }

    public virtual TbCompany? Company { get; set; }

    public virtual ICollection<TbEmployeeActivity> TbEmployeeActivities { get; set; } = new List<TbEmployeeActivity>();
}
