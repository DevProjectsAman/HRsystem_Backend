using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.VacationType.GetAllVacationTypes;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace HRsystem.Api.Features.Organization.WorkLocation.GetAllWorkLocationsQuery
{
    public record GetAllWorkLocationsQuery() : IRequest<List<WorkLocationDto>>;

    public class WorkLocationDto
    {
        public int WorkLocationId { get; set; }
        public int CompanyId { get; set; }
        public string WorkLocationCode { get; set; }
        public string LocationName { get; set; }
        public int? CityId { get; set; }
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
            var statuses = await _db.TbWorkLocations.ToListAsync(ct);

            var lang = _CurrentUser.UserLanguage ?? "en";

            return statuses.Select(s => new WorkLocationDto
            {
                WorkLocationId = s.WorkLocationId,
                CompanyId = s.CompanyId,
                LocationName = s.LocationName.GetTranslation(lang),// ✅ translated here
                CityId = s.CityId,
                WorkLocationCode = s.WorkLocationCode
            }).ToList();

        }

    }
}
