using FluentValidation;
using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.AuditLog.UpdateAuditLog
{
    public record UpdateAuditLogCommand(
        long AuditId,
        int CompanyId,
        int UserId,
        DateTime ActionDatetime,
        string TableName,
        string ActionType,
        string RecordId,
        string? OldData,
        string? NewData
    ) : IRequest<AuditLogResponse?>;

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

    public class UpdateAuditLogHandler : IRequestHandler<UpdateAuditLogCommand, AuditLogResponse?>
    {
        private readonly DBContextHRsystem _db;

        public UpdateAuditLogHandler(DBContextHRsystem db) => _db = db;

        public async Task<AuditLogResponse?> Handle(UpdateAuditLogCommand request, CancellationToken ct)
        {
            var entity = await _db.TbAuditLogs.FirstOrDefaultAsync(x => x.AuditId == request.AuditId, ct);

            if (entity == null)
            {
                throw new KeyNotFoundException($"Audit Log with ID {request.AuditId} not found.");
            }

            entity.CompanyId = request.CompanyId;
            entity.UserId = request.UserId;
            entity.ActionDatetime = request.ActionDatetime;
            entity.TableName = request.TableName;
            entity.ActionType = request.ActionType;
            entity.RecordId = request.RecordId;
            entity.OldData = request.OldData;
            entity.NewData = request.NewData;

            await _db.SaveChangesAsync(ct);

            return new AuditLogResponse(
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

    public class UpdateAuditLogValidator : AbstractValidator<UpdateAuditLogCommand>
    {
        public UpdateAuditLogValidator()
        {
            RuleFor(x => x.AuditId).GreaterThan(0);
            RuleFor(x => x.CompanyId).GreaterThan(0);
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.TableName).NotEmpty().MaximumLength(25);
            RuleFor(x => x.ActionType).NotEmpty().MaximumLength(25);
            RuleFor(x => x.RecordId).NotEmpty().MaximumLength(25);
        }
    }
}
