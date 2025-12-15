using HRsystem.Api.Database;
using HRsystem.Api.Features.Organization.WorkLocation.GetAllWorkLocations;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.WorkLocation.GetSpecificWorkLocations
{
    public record GetSpecificWorkLocationsQuery(int companyId,int cityId) : IRequest<List<WorkLocationDto>>;

    

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
            var statuses = await _db.TbWorkLocations.Where(c=>c.CompanyId==request.companyId && c.CityId==request.cityId).ToListAsync(ct);

           // var lang = _CurrentUser.UserLanguage ?? "en";

            // get city name and gov name 
                var citi = await _db.TbCities.Where(c=>c.CityId==request.cityId)
                .Include(g=>g.Gov).FirstOrDefaultAsync();


            return statuses.Select(s => new WorkLocationDto
            {
                WorkLocationId = s.WorkLocationId,
                CompanyId = s.CompanyId,
                LocationName = s.LocationName,// ✅ translated here
                CityId = s.CityId,
                WorkLocationCode = s.WorkLocationCode,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                AllowedRadiusM = s.AllowedRadiusM,
                GovId = s.GovId,
                GovName= citi?.Gov?.GovName,
                CityName= citi?.CityName,


            }).ToList();

        }

    }
}
