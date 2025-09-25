using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.City.GetCityById
{
    public record GetCityByIdQuery(int CityId) : IRequest<TbCity?>;

    public class Handler(DBContextHRsystem db) : IRequestHandler<GetCityByIdQuery, TbCity?>
    {
        public async Task<TbCity?> Handle(GetCityByIdQuery request, CancellationToken ct)
        {
            return await db.TbCities
                .Include(c => c.Gov)
                .FirstOrDefaultAsync(x => x.CityId == request.CityId, ct);
        }
    }
}
