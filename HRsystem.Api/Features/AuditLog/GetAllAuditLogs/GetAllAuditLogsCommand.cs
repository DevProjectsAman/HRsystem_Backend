using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.AuditLog.GetAllAuditLogs
{
    public record GetAllAuditLogsCommand() : IRequest<List<AuditLogResponse>>;

    public record AuditLogResponse(
        long AuditId,
        int CompanyId,
        int UserId,
        DateTime ActionDatetime,
        string TableName,
        string ActionType,
        string RecordId,
        string? OldData,
        string? NewData
    );

    public class GetAllAuditLogsHandler : IRequestHandler<GetAllAuditLogsCommand, List<AuditLogResponse>>
    {
        private readonly DBContextHRsystem _db;

        public GetAllAuditLogsHandler(DBContextHRsystem db) => _db = db;

        public async Task<List<AuditLogResponse>> Handle(GetAllAuditLogsCommand request, CancellationToken ct)
        {
            return await _db.TbAuditLogs
                .Select(entity => new AuditLogResponse(
                    entity.AuditId,
                    entity.CompanyId,
                    entity.UserId,
                    entity.ActionDatetime,
                    entity.TableName,
                    entity.ActionType,
                    entity.RecordId,
                    entity.OldData,
                    entity.NewData
                ))
                .ToListAsync(ct);
        }
    }
}
