using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace HRsystem.Api.Features.ShiftRule.UpdateShiftRule
{
    public record UpdateShiftRuleCommand(
        int RuleId,
        int? JobTitleId,
        int? WorkingLocationId,
        int? ProjectId,
        int ShiftId,
        int? Priority,
        int CompanyId,
        int? UpdatedBy
    ) : IRequest<TbShiftRule?>;

    public class UpdateShiftRuleHandler : IRequestHandler<UpdateShiftRuleCommand, TbShiftRule?>
    {
        private readonly DBContextHRsystem _db;
        public UpdateShiftRuleHandler(DBContextHRsystem db) => _db = db;

        public async Task<TbShiftRule?> Handle(UpdateShiftRuleCommand request, CancellationToken ct)
        {
            var entity = await _db.TbShiftRules.FirstOrDefaultAsync(r => r.RuleId == request.RuleId, ct);
            if (entity == null) return null;

            entity.JobTitleId = request.JobTitleId;
            entity.WorkingLocationId = request.WorkingLocationId;
            entity.ProjectId = request.ProjectId;
            entity.ShiftId = request.ShiftId;
            entity.Priority = request.Priority;
            entity.CompanyId = request.CompanyId;
            entity.UpdatedBy = request.UpdatedBy;
            entity.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
            return entity;
        }
    }
}
