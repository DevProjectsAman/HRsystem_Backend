using System;
using System.Collections.Generic;

namespace HRsystem.Api.Database.DataTables;

public partial class TbAuditLog
{
    public long AuditId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public DateTime ActionDatetime { get; set; }

    public string TableName { get; set; } = null!;

    public string ActionType { get; set; } = null!;

    public string RecordId { get; set; } = null!;

    public string? OldData { get; set; }

    public string? NewData { get; set; }

    public virtual TbCompany Company { get; set; } = null!;
}
