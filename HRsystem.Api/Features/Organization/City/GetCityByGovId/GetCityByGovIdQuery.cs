using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.City.GetCityByGovId
{
    // ✅ Change return type to CityDto
    public record GetCityByGovIdQuery(int GovId) : IRequest<List<CityDto?>>;

    public class Handler(DBContextHRsystem db) : IRequestHandler<GetCityByGovIdQuery, List<CityDto?>>
    {
        public async Task<List<CityDto?>> Handle(GetCityByGovIdQuery request, CancellationToken ct)
        {
            // ✅ Use projection to select only needed fields
           var res =  await db.TbCities
                .Where(x => x.GovId == request.GovId)
                .Select(x => new CityDto
                {
                    CityId = x.CityId,
                    GovId = x.GovId,
                    CityName = x.CityName
                })
                .ToListAsync(ct);
      
        return res; 

        }
    }

    // ✅ DTO definition
    public class CityDto
    {
        public int CityId { get; set; }
        public int? GovId { get; set; }
        public string CityName { get; set; } = string.Empty;
    }
}
