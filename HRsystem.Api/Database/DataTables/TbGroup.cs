using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbGroup
{
    public int GroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public virtual ICollection<TbCompany> TbCompanies { get; set; } = new List<TbCompany>();
}
