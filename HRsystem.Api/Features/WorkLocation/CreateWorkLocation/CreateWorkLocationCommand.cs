using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Shared.DTO;
using MediatR;

namespace HRsystem.Api.Features.WorkLocation.CreateWorkLocation
{
    public record CreateWorkLocationCommand(
        int CompanyId,
        string? WorkLocationCode,
        LocalizedData LocationName,
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

namespace HRsystem.Api.Features.WorkLocation.CreateWorkLocation
{
    public class CreateWorkLocationValidator : AbstractValidator<CreateWorkLocationCommand>
    {
        public CreateWorkLocationValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("CompanyId is required");

            RuleFor(x => x.LocationName.en)
                .NotEmpty().WithMessage("LocationName is required")
                .MaximumLength(200).WithMessage("LocationName must not exceed 200 characters");
             RuleFor(x => x.LocationName.ar)
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
