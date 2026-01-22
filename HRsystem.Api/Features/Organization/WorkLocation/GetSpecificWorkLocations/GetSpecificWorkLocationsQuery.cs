using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Organization.WorkLocation.GetAllWorkLocations;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.WorkLocation.GetSpecificWorkLocations
{
    public record GetSpecificWorkLocationsQuery(int companyId, int govId, int cityId) : IRequest<List<WorkLocationDto>>;



    public class Handler : IRequestHandler<GetSpecificWorkLocationsQuery, List<WorkLocationDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _CurrentUser;
        public Handler(DBContextHRsystem db, ICurrentUserService CurrentUser)
        {
            _db = db;
            _CurrentUser = CurrentUser;
        }


        public async Task<List<WorkLocationDto>> Handle(GetSpecificWorkLocationsQuery request, CancellationToken ct)
        {
            var query = _db.TbWorkLocations
                .AsNoTracking()
                .Where(w => w.CompanyId == request.companyId)
                .Include(w => w.City)
                    .ThenInclude(c => c.Gov).AsQueryable();

            // 🔹 Filtering logic
            if (request.cityId > 0)
            {
                query = query.Where(w => w.CityId == request.cityId);
            }
            else if (request.govId > 0)
            {
                // This returns ALL cities under the gov
                query = query.Where(w => w.City.GovId == request.govId);
            }

            // 🔹 Projection + Sorting
            return await query
                .OrderBy(w => w.City.CityName) // ✅ sort by city name
                .Select(w => new WorkLocationDto
                {
                    WorkLocationId = w.WorkLocationId,
                    CompanyId = w.CompanyId,
                    LocationName = w.LocationName,
                    WorkLocationCode = w.WorkLocationCode,
                    Latitude = w.Latitude,
                    Longitude = w.Longitude,
                    AllowedRadiusM = w.AllowedRadiusM,

                    CityId = w.CityId,
                    CityName = w.City.CityName,     // ✅ works even when cityId = 0

                    GovId = w.City.GovId,
                    GovName = w.City.Gov.GovName
                })
                .ToListAsync(ct);
        }



    }
}
