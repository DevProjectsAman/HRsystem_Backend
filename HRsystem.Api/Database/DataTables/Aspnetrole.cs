using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class Aspnetrole
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<Aspnetroleclaim> Aspnetroleclaims { get; set; } = new List<Aspnetroleclaim>();

    public virtual ICollection<Asprolepermission> Asprolepermissions { get; set; } = new List<Asprolepermission>();

    public virtual ICollection<Aspnetuser> Users { get; set; } = new List<Aspnetuser>();
}
