using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace HRsystem.Api.Features.EmployeeUpdates.GetEmployeeWorkLocationsTree
{

    public enum SelectionState
    {
        None = 0,       // No child selected
        Partial = 1,    // Some selected
        All = 2         // All selected
    }

    public class GovWithCitiesDto
    {
        public int GovId { get; set; }
        public string GovName { get; set; } = string.Empty;
        public SelectionState SelectionState { get; set; }

        public List<CityWithWorkLocationsDto> Cities { get; set; } = new();
    }

    public class CityWithWorkLocationsDto
    {
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public SelectionState SelectionState { get; set; }


        public List<WorkLocationSelectionDto> WorkLocations { get; set; } = new();
    }

    public class WorkLocationSelectionDto
    {
        public int WorkLocationId { get; set; }
        public int CompanyId { get; set; }
        public string WorkLocationCode { get; set; } = string.Empty;
        public LocalizedData LocationName { get; set; } = new();

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? AllowedRadiusM { get; set; }

        public int? GovId { get; set; }
        public int? CityId { get; set; }

        public string? CityName { get; set; }
        public string? GovName { get; set; }

        // ⭐ TreeView
        public bool IsSelected { get; set; }

    }

    public record GetWorkLocationsHierarchyQuery(int employeeId) : IRequest<List<GovWithCitiesDto>>;

 
        public class Handler : IRequestHandler<GetWorkLocationsHierarchyQuery, List<GovWithCitiesDto>>
        {
            private readonly DBContextHRsystem _db;
            private readonly ICurrentUserService _currentUser;

            public Handler(DBContextHRsystem db, ICurrentUserService currentUser)
            {
                _db = db;
                _currentUser = currentUser;
            }

            public async Task<List<GovWithCitiesDto>> Handle(GetWorkLocationsHierarchyQuery request, CancellationToken ct)
            {
                // Get selected work locations for the employee
                var selectedWorkLocationIds = await _db.TbEmployeeWorkLocations
                    .AsNoTracking()
                    .Where(e => e.EmployeeId == request.employeeId && e.CompanyId == _currentUser.CompanyID)
                    .Select(e => e.WorkLocationId)
                    .ToHashSetAsync(ct);

                // Load all work locations for the company
                var workLocations = await _db.TbWorkLocations
                    .AsNoTracking()
                    .Include(w => w.Gov)
                    .Include(w => w.City)
                    .Where(w => w.CompanyId == _currentUser.CompanyID)
                    .ToListAsync(ct);

                // Group by Gov (null-safe)
                var govs = workLocations
                    .GroupBy(w => new
                    {
                        w.GovId,
                        GovName = w.Gov?.GovName ?? string.Empty
                    })
                    .Select(govGroup =>
                    {
                        var govDto = new GovWithCitiesDto
                        {
                            GovId = govGroup.Key.GovId ?? 0,
                            GovName = govGroup.Key.GovName
                        };

                        // Group by City (null-safe)
                        govDto.Cities = govGroup
                            .GroupBy(w => new
                            {
                                w.CityId,
                                CityName = w.City?.CityName ?? string.Empty
                            })
                            .Select(cityGroup =>
                            {
                                var cityDto = new CityWithWorkLocationsDto
                                {
                                    CityId = cityGroup.Key.CityId ?? 0,
                                    CityName = cityGroup.Key.CityName
                                };

                                // Map WorkLocations
                                cityDto.WorkLocations = cityGroup.Select(w => new WorkLocationSelectionDto
                                {
                                    WorkLocationId = w.WorkLocationId,
                                    CompanyId = w.CompanyId,
                                    WorkLocationCode = w.WorkLocationCode ?? string.Empty,
                                    LocationName = w.LocationName,
                                    Latitude = w.Latitude,
                                    Longitude = w.Longitude,
                                    AllowedRadiusM = w.AllowedRadiusM,
                                    GovId = w.GovId,
                                    CityId = w.CityId,
                                    CityName= w.City?.CityName,
                                    GovName= w.Gov?.GovName,
                                    IsSelected = selectedWorkLocationIds.Contains(w.WorkLocationId)
                                }).ToList();

                                // City selection state
                                var selectedCount = cityDto.WorkLocations.Count(w => w.IsSelected);
                                cityDto.SelectionState =
                                    selectedCount == 0
                                        ? SelectionState.None
                                        : selectedCount == cityDto.WorkLocations.Count
                                            ? SelectionState.All
                                            : SelectionState.Partial;

                                return cityDto;
                            })
                            .ToList();

                        // Gov selection state
                        var cityStates = govDto.Cities.Select(c => c.SelectionState).ToList();
                        govDto.SelectionState =
                            cityStates.All(s => s == SelectionState.None)
                                ? SelectionState.None
                                : cityStates.All(s => s == SelectionState.All)
                                    ? SelectionState.All
                                    : SelectionState.Partial;

                        return govDto;
                    })
                    .ToList();

                return govs;
            }
        }
    }




 
