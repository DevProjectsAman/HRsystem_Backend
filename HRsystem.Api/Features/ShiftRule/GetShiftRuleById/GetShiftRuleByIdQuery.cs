using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace HRsystem.Api.Features.ShiftRule.GetShiftRuleById
{
    public record GetShiftRuleByIdQuery(int RuleId) : IRequest<TbShiftRule?>;

    public class GetShiftRuleByIdHandler : IRequestHandler<GetShiftRuleByIdQuery, TbShiftRule?>
    {
        private readonly DBContextHRsystem _db;
        public GetShiftRuleByIdHandler(DBContextHRsystem db) => _db = db;

        public async Task<TbShiftRule?> Handle(GetShiftRuleByIdQuery request, CancellationToken ct)
        {
            return await _db.TbShiftRules.FirstOrDefaultAsync(r => r.RuleId == request.RuleId, ct);
        }
    }
}
