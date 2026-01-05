using HRsystem.Api.Database;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace HRsystem.Api.Features.Scheduling.ShiftRule.DeleteShiftRule
{
    public record DeleteShiftRuleCommand(int RuleId) : IRequest<bool>;

    public class DeleteShiftRuleHandler : IRequestHandler<DeleteShiftRuleCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public DeleteShiftRuleHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteShiftRuleCommand request, CancellationToken ct)
        {
            var entity = await _db.TbShiftRules.FirstOrDefaultAsync(r => r.RuleId == request.RuleId, ct);
            if (entity == null) return false;

            _db.TbShiftRules.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
