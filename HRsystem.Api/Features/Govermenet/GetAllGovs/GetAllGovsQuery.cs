using HRsystem.Api.Database.DataTables;
using MediatR;
using HRsystem.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Gov.GetAllGovs
{
    public record GetAllGovsQuery() : IRequest<List<TbGov>>;

    public class Handler : IRequestHandler<GetAllGovsQuery, List<TbGov>>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<List<TbGov>> Handle(GetAllGovsQuery request, CancellationToken ct)
        {
            return await _db.TbGovs.ToListAsync(ct);
        }
    }
}
