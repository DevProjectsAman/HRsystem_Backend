using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.AuditLog.DeleteAuditLog
{
    public record DeleteAuditLogCommand(long AuditId) : IRequest<bool>;

    public class DeleteAuditLogHandler : IRequestHandler<DeleteAuditLogCommand, bool>
    {
        private readonly DBContextHRsystem _db;

        public DeleteAuditLogHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteAuditLogCommand request, CancellationToken ct)
        {
            var entity = await _db.TbAuditLogs.FirstOrDefaultAsync(x => x.AuditId == request.AuditId, ct);

            if (entity == null) return false;

            _db.TbAuditLogs.Remove(entity);
            await _db.SaveChangesAsync(ct);

            return true;
        }
    }
}
