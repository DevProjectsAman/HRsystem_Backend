using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.WorkLocation.GetAllWorkLocations
{
    public record GetAllWorkLocationsQuery() : IRequest<List<TbWorkLocation>>;

    public class Handler : IRequestHandler<GetAllWorkLocationsQuery, List<TbWorkLocation>>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<List<TbWorkLocation>> Handle(GetAllWorkLocationsQuery request, CancellationToken ct)
            => await _db.TbWorkLocations.ToListAsync(ct);
    }
}
