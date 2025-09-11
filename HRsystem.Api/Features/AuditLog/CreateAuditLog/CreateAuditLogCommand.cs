using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;

namespace HRsystem.Api.Features.AuditLog.CreateAuditLog
{
    public record CreateAuditLogCommand(
        int CompanyId,
        int UserId,
        DateTime ActionDatetime,
        string TableName,
        string ActionType,
        string RecordId,
        string? OldData,
        string? NewData
    ) : IRequest<AuditLogResponse>;

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

    public class CreateAuditLogHandler : IRequestHandler<CreateAuditLogCommand, AuditLogResponse>
    {
        private readonly DBContextHRsystem _db;

        public CreateAuditLogHandler(DBContextHRsystem db) => _db = db;

        public async Task<AuditLogResponse> Handle(CreateAuditLogCommand request, CancellationToken ct)
        {
            var entity = new TbAuditLog
            {
                CompanyId = request.CompanyId,
                UserId = request.UserId,
                ActionDatetime = request.ActionDatetime,
                TableName = request.TableName,
                ActionType = request.ActionType,
                RecordId = request.RecordId,
                OldData = request.OldData,
                NewData = request.NewData
            };

            _db.TbAuditLogs.Add(entity);
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

    public class CreateAuditLogValidator : AbstractValidator<CreateAuditLogCommand>
    {
        public CreateAuditLogValidator()
        {
            RuleFor(x => x.CompanyId).GreaterThan(0);
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.TableName).NotEmpty().MaximumLength(25);
            RuleFor(x => x.ActionType).NotEmpty().MaximumLength(25);
            RuleFor(x => x.RecordId).NotEmpty().MaximumLength(25);
        }
    }
}
