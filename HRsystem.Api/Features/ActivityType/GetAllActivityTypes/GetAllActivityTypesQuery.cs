using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.ActivityType.GetAllActivityTypes
{
    public record GetAllActivityTypesQuery() : IRequest<List<TbActivityType>>;

    public class Handler : IRequestHandler<GetAllActivityTypesQuery, List<TbActivityType>>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<List<TbActivityType>> Handle(GetAllActivityTypesQuery request, CancellationToken ct)
        {
            return await _db.TbActivityTypes.ToListAsync(ct);
        }
    }
}
