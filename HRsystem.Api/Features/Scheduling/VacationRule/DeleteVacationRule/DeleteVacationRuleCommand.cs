using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Scheduling.VacationRule.DeleteVacationRule
{
    public record DeleteVacationRuleCommand(int RuleId) : IRequest<bool>;

    public class DeleteVacationRuleHandler : IRequestHandler<DeleteVacationRuleCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public DeleteVacationRuleHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteVacationRuleCommand request, CancellationToken ct)
        {
            var entity = await _db.TbVacationRules.FirstOrDefaultAsync(r => r.RuleId == request.RuleId, ct);
            if (entity == null) return false;

            _db.TbVacationRules.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
