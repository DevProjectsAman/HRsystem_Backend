using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.City.GetCityById
{
    public record GetCityByCityIdQuery(int CityId) : IRequest<TbCity?>;

    public class Handler(DBContextHRsystem db) : IRequestHandler<GetCityByCityIdQuery, TbCity?>
    {
        public async Task<TbCity?> Handle(GetCityByCityIdQuery request, CancellationToken ct)
        {
            return await db.TbCities
                .Include(c => c.Gov)
                .FirstOrDefaultAsync(x => x.CityId == request.CityId, ct);
        }
    }
}
