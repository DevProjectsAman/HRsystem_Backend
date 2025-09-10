using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.WorkLocation.UpdateWorkLocation
{
    public record UpdateWorkLocationCommand(
        int WorkLocationId,
        int CompanyId,
        string? WorkLocationCode,
        string LocationName,
        decimal? Latitude,
        decimal? Longitude,
        int? AllowedRadiusM,
        int? CityId
    ) : IRequest<TbWorkLocation?>;

    public class Handler : IRequestHandler<UpdateWorkLocationCommand, TbWorkLocation?>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbWorkLocation?> Handle(UpdateWorkLocationCommand request, CancellationToken ct)
        {
            var entity = await _db.TbWorkLocations
                .FirstOrDefaultAsync(x => x.WorkLocationId == request.WorkLocationId, ct);

            if (entity == null) return null;

            entity.CompanyId = request.CompanyId;
            entity.WorkLocationCode = request.WorkLocationCode;
            entity.LocationName = request.LocationName;
            entity.Latitude = request.Latitude;
            entity.Longitude = request.Longitude;
            entity.AllowedRadiusM = request.AllowedRadiusM;
            entity.CityId = request.CityId;
            entity.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
            return entity;
        }
    }
}
