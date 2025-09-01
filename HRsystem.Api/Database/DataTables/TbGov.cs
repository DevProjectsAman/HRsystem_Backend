using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbGov
{
    public int GovId { get; set; }

    public string? GovName { get; set; }

    public string? GovArea { get; set; }

    public virtual ICollection<TbCity> TbCities { get; set; } = new List<TbCity>();
}
