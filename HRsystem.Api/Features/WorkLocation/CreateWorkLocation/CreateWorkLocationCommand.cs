using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;

namespace HRsystem.Api.Features.WorkLocation.CreateWorkLocation
{
    public record CreateWorkLocationCommand(
        int CompanyId,
        string? WorkLocationCode,
        string LocationName,
        decimal? Latitude,
        decimal? Longitude,
        int? AllowedRadiusM,
        int? CityId
    ) : IRequest<TbWorkLocation>;

    public class Handler : IRequestHandler<CreateWorkLocationCommand, TbWorkLocation>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbWorkLocation> Handle(CreateWorkLocationCommand request, CancellationToken ct)
        {
            var entity = new TbWorkLocation
            {
                CompanyId = request.CompanyId,
                WorkLocationCode = request.WorkLocationCode,
                LocationName = request.LocationName,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                AllowedRadiusM = request.AllowedRadiusM,
                CityId = request.CityId,
                CreatedAt = DateTime.UtcNow
            };

            _db.TbWorkLocations.Add(entity);
            await _db.SaveChangesAsync(ct);

            return entity;
        }
    }
}
