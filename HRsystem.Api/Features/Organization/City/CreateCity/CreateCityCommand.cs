using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;

namespace HRsystem.Api.Features.Organization.City.CreateCity
{
    public record CreateCityCommand(string CityName, int? GovId, int? CreatedBy) : IRequest<TbCity>;

    public class Handler(DBContextHRsystem db) : IRequestHandler<CreateCityCommand, TbCity>
    {
        public async Task<TbCity> Handle(CreateCityCommand request, CancellationToken ct)
        {
            var entity = new TbCity
            {
                CityName = request.CityName,
                GovId = request.GovId,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            db.TbCities.Add(entity);
            await db.SaveChangesAsync(ct);
            return entity;
        }
    }
}


namespace HRsystem.Api.Features.Organization.City.CreateCity
{
    public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
    {
        public CreateCityCommandValidator()
        {
            RuleFor(x => x.CityName)
                .NotEmpty().WithMessage("City name is required")
                .MaximumLength(75).WithMessage("City name cannot exceed 75 characters");

            RuleFor(x => x.GovId)
                .NotNull().WithMessage("GovId is required");
        }
    }
}