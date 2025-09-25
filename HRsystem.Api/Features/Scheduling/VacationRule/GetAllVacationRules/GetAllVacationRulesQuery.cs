using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Scheduling.VacationRule.GetAllVacationRules
{
    public record GetAllVacationRulesQuery() : IRequest<List<TbVacationRule>>;

    public class GetAllVacationRulesHandler : IRequestHandler<GetAllVacationRulesQuery, List<TbVacationRule>>
    {
        private readonly DBContextHRsystem _db;
        public GetAllVacationRulesHandler(DBContextHRsystem db) => _db = db;

        public async Task<List<TbVacationRule>> Handle(GetAllVacationRulesQuery request, CancellationToken ct)
        {
            return await _db.TbVacationRules.ToListAsync(ct);
        }
    }
}
