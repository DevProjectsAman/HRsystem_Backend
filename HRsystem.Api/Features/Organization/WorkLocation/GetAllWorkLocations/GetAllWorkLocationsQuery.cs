using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.WorkLocation.GetAllWorkLocations
{
    public record GetAllWorkLocationsQuery() : IRequest<List<WorkLocationDto>>;

    public class WorkLocationDto
    {
        public int WorkLocationId { get; set; }
        public int CompanyId { get; set; }
        public string WorkLocationCode { get; set; }
        public LocalizedData LocationName { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public int? AllowedRadiusM { get; set; }
        
        public int? GovId { get; set; }
        public string? GovName { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }

    }

    public class Handler : IRequestHandler<GetAllWorkLocationsQuery, List<WorkLocationDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _CurrentUser;
        public Handler(DBContextHRsystem db, ICurrentUserService CurrentUser)
        {
            _db = db;
            _CurrentUser = CurrentUser;
        }

        public async Task<List<WorkLocationDto>> Handle(GetAllWorkLocationsQuery request, CancellationToken ct)
        {
            var statuses = await _db.TbWorkLocations
                .Include(r=>r.City)
                .Include(r=>r.Gov)
                .ToListAsync(ct);

            var lang = _CurrentUser.UserLanguage ?? "en";


            var res =  statuses.Select(s => new WorkLocationDto
            {
                WorkLocationId = s.WorkLocationId,
                CompanyId = s.CompanyId,
                LocationName = s.LocationName,// ✅ translated here
                CityId = s.CityId,
                CityName = s.City.CityName,
                GovId = s.GovId,
                GovName = s.Gov.GovName,

                WorkLocationCode = s.WorkLocationCode,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                AllowedRadiusM = s.AllowedRadiusM,

            }).ToList();


            return res;
        }

    }
}
