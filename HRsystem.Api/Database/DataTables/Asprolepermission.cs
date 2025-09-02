using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class Asprolepermission
{
    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual Asppermission Permission { get; set; } = null!;

    public virtual Aspnetrole Role { get; set; } = null!;
}
