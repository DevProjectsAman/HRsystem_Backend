using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbActivityType
{
    public int ActivityTypeId { get; set; }

    public string ActivityCode { get; set; } = null!;

    public string ActivityName { get; set; } = null!;

    public string? ActivityDescription { get; set; }

    public int? CompanyId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public virtual TbCompany? Company { get; set; }

    public virtual ICollection<TbEmployeeActivity> TbEmployeeActivities { get; set; } = new List<TbEmployeeActivity>();
}
