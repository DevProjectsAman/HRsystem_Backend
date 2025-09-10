using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.City.UpdateCity
{
    public record UpdateCityCommand(int CityId, string CityName, int? GovId, int? UpdatedBy) : IRequest<TbCity?>;

    public class Handler(DBContextHRsystem db) : IRequestHandler<UpdateCityCommand, TbCity?>
    {
        public async Task<TbCity?> Handle(UpdateCityCommand request, CancellationToken ct)
        {
            var entity = await db.TbCities.FirstOrDefaultAsync(x => x.CityId == request.CityId, ct);
            if (entity == null) return null;

            entity.CityName = request.CityName;
            entity.GovId = request.GovId;
            entity.UpdatedBy = request.UpdatedBy;
            entity.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync(ct);
            return entity;
        }
    }
}


namespace HRsystem.Api.Features.City.UpdateCity
{
    public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
    {
        public UpdateCityCommandValidator()
        {
            RuleFor(x => x.CityId).GreaterThan(0);

            RuleFor(x => x.CityName)
                .NotEmpty().WithMessage("City name is required")
                .MaximumLength(75).WithMessage("City name cannot exceed 75 characters");

            RuleFor(x => x.GovId)
                .NotNull().WithMessage("GovId is required");
        }
    }
}