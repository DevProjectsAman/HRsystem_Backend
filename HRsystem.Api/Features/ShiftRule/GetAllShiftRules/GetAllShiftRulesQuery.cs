using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace HRsystem.Api.Features.ShiftRule.GetAllShiftRules
{
    public record GetAllShiftRulesQuery() : IRequest<List<TbShiftRule>>;

    public class GetAllShiftRulesHandler : IRequestHandler<GetAllShiftRulesQuery, List<TbShiftRule>>
    {
        private readonly DBContextHRsystem _db;
        public GetAllShiftRulesHandler(DBContextHRsystem db) => _db = db;

        public async Task<List<TbShiftRule>> Handle(GetAllShiftRulesQuery request, CancellationToken ct)
        {
            return await _db.TbShiftRules.ToListAsync(ct);
        }
    }
}
