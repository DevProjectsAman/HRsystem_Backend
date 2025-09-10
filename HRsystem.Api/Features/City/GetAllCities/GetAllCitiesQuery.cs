using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.City.GetAllCities
{
    public record GetAllCitiesQuery() : IRequest<List<TbCity>>;

    public class Handler(DBContextHRsystem db) : IRequestHandler<GetAllCitiesQuery, List<TbCity>>
    {
        public async Task<List<TbCity>> Handle(GetAllCitiesQuery request, CancellationToken ct)
        {
            return await db.TbCities
                .Include(c => c.Gov)
                .ToListAsync(ct);
        }
    }
}
