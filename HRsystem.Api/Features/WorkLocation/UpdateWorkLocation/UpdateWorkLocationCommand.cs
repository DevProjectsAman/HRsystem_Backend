using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.WorkLocation.UpdateWorkLocation
{
    public record UpdateWorkLocationCommand(
        int WorkLocationId,
        int CompanyId,
        string? WorkLocationCode,
        LocalizedData LocationName,
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
namespace HRsystem.Api.Features.WorkLocation.UpdateWorkLocation
{
    public class UpdateWorkLocationValidator : AbstractValidator<UpdateWorkLocationCommand>
    {
        public UpdateWorkLocationValidator()
        {
            RuleFor(x => x.WorkLocationId)
                .GreaterThan(0).WithMessage("WorkLocationId is required");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("CompanyId is required");

            RuleFor(x => x.LocationName.En)
                .NotEmpty().WithMessage("LocationName is required")
                .MaximumLength(200).WithMessage("LocationName must not exceed 200 characters");

            RuleFor(x => x.LocationName.Ar)
               .NotEmpty().WithMessage("LocationName is required")
               .MaximumLength(200).WithMessage("LocationName must not exceed 200 characters");

            RuleFor(x => x.WorkLocationCode)
                .MaximumLength(50).WithMessage("WorkLocationCode must not exceed 50 characters");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).When(x => x.Latitude.HasValue)
                .WithMessage("Latitude must be between -90 and 90");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).When(x => x.Longitude.HasValue)
                .WithMessage("Longitude must be between -180 and 180");

            RuleFor(x => x.AllowedRadiusM)
                .GreaterThan(0).When(x => x.AllowedRadiusM.HasValue)
                .WithMessage("AllowedRadiusM must be greater than 0");
        }
    }
}
