using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.AuditLog.GetAuditLogById
{
    public record GetAuditLogByIdCommand(long AuditId) : IRequest<AuditLogResponse?>;

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

    public class GetAuditLogByIdHandler : IRequestHandler<GetAuditLogByIdCommand, AuditLogResponse?>
    {
        private readonly DBContextHRsystem _db;

        public GetAuditLogByIdHandler(DBContextHRsystem db) => _db = db;

        public async Task<AuditLogResponse?> Handle(GetAuditLogByIdCommand request, CancellationToken ct)
        {
            var entity = await _db.TbAuditLogs.FirstOrDefaultAsync(x => x.AuditId == request.AuditId, ct);

            return entity == null
                ? null
                : new AuditLogResponse(
                    entity.AuditId,
                    entity.CompanyId,
                    entity.UserId,
                    entity.ActionDatetime,
                    entity.TableName,
                    entity.ActionType,
                    entity.RecordId,
                    entity.OldData,
                    entity.NewData
                );
        }
    }
}
