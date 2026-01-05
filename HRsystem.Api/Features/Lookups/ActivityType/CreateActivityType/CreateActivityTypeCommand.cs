using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Shared.DTO;
using MediatR;

namespace HRsystem.Api.Features.Lookups.ActivityType.CreateActivityType
{
    public record CreateActivityTypeCommand(
        string ActivityCode,
        LocalizedData ActivityName,
        string? ActivityDescription,
        int? CompanyId,
        int? CreatedBy
    ) : IRequest<TbActivityType>;

    public class Handler : IRequestHandler<CreateActivityTypeCommand, TbActivityType>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbActivityType> Handle(CreateActivityTypeCommand request, CancellationToken ct)
        {
            var entity = new TbActivityType
            {
                ActivityCode = request.ActivityCode,
                ActivityName = request.ActivityName,
                ActivityDescription = request.ActivityDescription,
                CompanyId = request.CompanyId,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            _db.TbActivityTypes.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }
    }
}




namespace HRsystem.Api.Features.Lookups.ActivityType.CreateActivityType
{
    public class CreateActivityTypeValidator : AbstractValidator<CreateActivityTypeCommand>
    {
        public CreateActivityTypeValidator()
        {
            RuleFor(x => x.ActivityCode)
                .NotEmpty().WithMessage("Activity code is required")
                .MaximumLength(25).WithMessage("Activity code cannot exceed 25 characters");

            RuleFor(x => x.ActivityName.en)
                .NotEmpty().WithMessage("Activity name is required")
                .MaximumLength(55).WithMessage("Activity name cannot exceed 55 characters");

            RuleFor(x => x.ActivityName.ar)
               .NotEmpty().WithMessage("Activity name is required")
               .MaximumLength(55).WithMessage("Activity name cannot exceed 55 characters");

            RuleFor(x => x.ActivityDescription)
                .MaximumLength(75).WithMessage("Activity description cannot exceed 75 characters")
                .When(x => !string.IsNullOrEmpty(x.ActivityDescription));

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).When(x => x.CompanyId.HasValue)
                .WithMessage("Invalid CompanyId");
        }
    }
}


