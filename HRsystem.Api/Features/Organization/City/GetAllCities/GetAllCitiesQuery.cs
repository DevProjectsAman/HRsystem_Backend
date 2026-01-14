using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.City.GetAllCities
{
    public record GetAllCitiesQuery() : IRequest<List<CityDto>>;

    public class Handler(DBContextHRsystem db) : IRequestHandler<GetAllCitiesQuery, List<CityDto>>
    {
        public async Task<List<CityDto>> Handle(GetAllCitiesQuery request, CancellationToken ct)
        {
            return await db.TbCities
        .Include(c => c.Gov)
        .Select(c => new CityDto
        {
            CityId = c.CityId,
            CityName = c.CityName,
            GovId = c.GovId,
            GovName = c.Gov != null ? c.Gov.GovName : null
        })
        .ToListAsync(ct);
        }
    }


    public class CityDto
    {
        public int CityId { get; set; }
        public string? CityName { get; set; }
        public int? GovId { get; set; }
        public string? GovName { get; set; }
    }
}
