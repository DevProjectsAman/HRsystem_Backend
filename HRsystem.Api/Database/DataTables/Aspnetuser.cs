using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class Aspnetuser
{
    public int Id { get; set; }

    public Guid RowGuid { get; set; }

    public string UserFullName { get; set; } = null!;

    public int CompanyId { get; set; }

    public bool IsActive { get; set; }

    public bool IsToChangePassword { get; set; }

    public DateTime? LastPasswordChangedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public DateTime? LastFailedLoginAt { get; set; }

    public bool ForceLogout { get; set; }

    public int FailedLoginCount { get; set; }

    public string PreferredLanguage { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public virtual ICollection<Aspnetuserclaim> Aspnetuserclaims { get; set; } = new List<Aspnetuserclaim>();

    public virtual ICollection<Aspnetuserlogin> Aspnetuserlogins { get; set; } = new List<Aspnetuserlogin>();

    public virtual ICollection<Aspnetusertoken> Aspnetusertokens { get; set; } = new List<Aspnetusertoken>();

    public virtual ICollection<Aspnetrole> Roles { get; set; } = new List<Aspnetrole>();
}
