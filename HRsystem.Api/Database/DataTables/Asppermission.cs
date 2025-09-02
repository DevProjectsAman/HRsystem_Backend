using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class Asppermission
{
    public int PermissionId { get; set; }

    public string PermissionCatagory { get; set; } = null!;

    public string PermissionName { get; set; } = null!;

    public string PermissionDescription { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual ICollection<Asprolepermission> Asprolepermissions { get; set; } = new List<Asprolepermission>();
}
