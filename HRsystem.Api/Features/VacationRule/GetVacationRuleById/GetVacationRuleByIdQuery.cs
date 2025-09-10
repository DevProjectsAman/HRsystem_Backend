using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.VacationRule.GetVacationRuleById
{
    public record GetVacationRuleByIdQuery(int RuleId) : IRequest<TbVacationRule?>;

    public class GetVacationRuleByIdHandler : IRequestHandler<GetVacationRuleByIdQuery, TbVacationRule?>
    {
        private readonly DBContextHRsystem _db;
        public GetVacationRuleByIdHandler(DBContextHRsystem db) => _db = db;

        public async Task<TbVacationRule?> Handle(GetVacationRuleByIdQuery request, CancellationToken ct)
        {
            return await _db.TbVacationRules.FirstOrDefaultAsync(r => r.RuleId == request.RuleId, ct);
        }
    }
}
