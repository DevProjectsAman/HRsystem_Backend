using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Holiday.GetAllHolidays;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static HRsystem.Api.Features.EmployeeUpdates.EmployeeWorkLocations.UpdateEmployeeWorkLocationsHandler;

namespace HRsystem.Api.Features.EmployeeUpdates
{
    public class EmployeeWorkLocations
    {
        #region  Get Employee Locations
        public record GetEmployeeWorkLocationsQuery(int employeeId) : IRequest<List<WorkLocationDto>>;

        public class WorkLocationDto
        {
            public int WorkLocationId { get; set; }


            public int CompanyId { get; set; }
            public string WorkLocationCode { get; set; }

            public LocalizedData LocationName { get; set; } = new LocalizedData();

            public double? Latitude { get; set; }

            public double? Longitude { get; set; }

            public int? AllowedRadiusM { get; set; }

            public int? CityId { get; set; }
            public string CityName { get; set; }
            public int? GovId { get; set; }
            public string GovName { get; set; }



        }

        public class GetEmployeeWorkLocationsHandler    : IRequestHandler<GetEmployeeWorkLocationsQuery, List<WorkLocationDto>>
        {
            private readonly DBContextHRsystem _db;

            public GetEmployeeWorkLocationsHandler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<List<WorkLocationDto>> Handle(
                GetEmployeeWorkLocationsQuery request,
                CancellationToken cancellationToken)
            {
                var workLocations = await _db.TbEmployeeWorkLocations
                    .AsNoTracking()
                    .Include(x => x.WorkLocation)
                    .Where(x => x.EmployeeId == request.employeeId)
                    .Select(x => new WorkLocationDto
                    {
                        WorkLocationId = x.WorkLocationId,
                        CompanyId = x.CompanyId,

                        WorkLocationCode = x.WorkLocation.WorkLocationCode,

                        LocationName = new LocalizedData
                        {
                            ar = x.WorkLocation.LocationName.ar,
                            en = x.WorkLocation.LocationName.en
                        },

                        Latitude = (double?)x.WorkLocation.Latitude,
                        Longitude = (double?)x.WorkLocation.Longitude,
                        AllowedRadiusM = x.WorkLocation.AllowedRadiusM,

                        CityId = x.CityId,
                        CityName = _db.TbCities
                            .Where(c => c.CityId == x.CityId)
                            .Select(c => c.CityName)
                            .FirstOrDefault(),

                        GovId = x.WorkLocation.GovId,
                        GovName = _db.TbGovs
                            .Where(g => g.GovId == x.WorkLocation.GovId)
                            .Select(g => g.GovName)
                            .FirstOrDefault()
                    })
                    .ToListAsync(cancellationToken);

                return workLocations;
            }
        }


        #endregion


        #region  Update Employee Locations

        public record UpdateEmployeeWorkLocationsCommand(
     int EmployeeId,
     List<WorkLocationRequestDto> WorkLocations
 ) : IRequest<bool>;

        public class WorkLocationRequestDto
        {
            public int WorkLocationId { get; set; }
            public int CityId { get; set; }
            public int CompanyId { get; set; }
        }

        public class UpdateEmployeeWorkLocationsHandler
     : IRequestHandler<UpdateEmployeeWorkLocationsCommand, bool>
        {
            private readonly DBContextHRsystem _db;
            private readonly ICurrentUserService _currentUserService;
            public UpdateEmployeeWorkLocationsHandler(DBContextHRsystem  db, ICurrentUserService currentUserService)
            {
                _db = db;
                _currentUserService = currentUserService;
            }

            public async Task<bool> Handle(
                UpdateEmployeeWorkLocationsCommand request,
                CancellationToken cancellationToken)
            {
                // ===== Validation =====
                if (request.EmployeeId <= 0)
                    throw new ValidationException("Invalid employee ID.");

                // ===== Transaction =====
                await using var transaction =
                    await _db.Database.BeginTransactionAsync(cancellationToken);

                try
                {
                    // ===== Delete existing work locations =====
                    var existing = await _db.TbEmployeeWorkLocations
                        .Where(x => x.EmployeeId == request.EmployeeId)
                        .ToListAsync(cancellationToken);

                    if (existing.Any())
                    {
                        _db.TbEmployeeWorkLocations.RemoveRange(existing);
                        await _db.SaveChangesAsync(cancellationToken);
                    }

                    // ===== Insert new work locations =====
                    if (request.WorkLocations != null && request.WorkLocations.Any())
                    {
                        var newEntities = request.WorkLocations.Select(w => new TbEmployeeWorkLocation
                        {
                            EmployeeId = request.EmployeeId,
                            WorkLocationId = w.WorkLocationId,
                            CityId = w.CityId,
                            CompanyId = w.CompanyId,

                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = _currentUserService.UserId // set from current user if available
                        }).ToList();

                        await _db.TbEmployeeWorkLocations.AddRangeAsync(newEntities, cancellationToken);
                        await _db.SaveChangesAsync(cancellationToken);
                    }

                    await transaction.CommitAsync(cancellationToken);
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }


        #endregion



    }

}

