using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.ActivityType.UpdateActivityType
{
    public record UpdateActivityTypeCommand(
        int ActivityTypeId,
        string ActivityCode,
        LocalizedData ActivityName,
        string? ActivityDescription,
        int? CompanyId
    ) : IRequest<TbActivityType?>;

    public class Handler : IRequestHandler<UpdateActivityTypeCommand, TbActivityType?>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbActivityType?> Handle(UpdateActivityTypeCommand request, CancellationToken ct)
        {
            var entity = await _db.TbActivityTypes.FirstOrDefaultAsync(x => x.ActivityTypeId == request.ActivityTypeId, ct);
            if (entity == null) return null;

            entity.ActivityCode = request.ActivityCode;
            entity.ActivityName = request.ActivityName;
            entity.ActivityDescription = request.ActivityDescription;
            entity.CompanyId = request.CompanyId;

            await _db.SaveChangesAsync(ct);
            return entity;
        }
    }


}


namespace HRsystem.Api.Features.ActivityType.UpdateActivityType
{
    public class UpdateActivityTypeValidator : AbstractValidator<UpdateActivityTypeCommand>
    {
        public UpdateActivityTypeValidator()
        {
            RuleFor(x => x.ActivityTypeId)
                .GreaterThan(0).WithMessage("ActivityTypeId is required");

            RuleFor(x => x.ActivityCode)
                .NotEmpty().WithMessage("Activity code is required")
                .MaximumLength(25).WithMessage("Activity code cannot exceed 25 characters");

            RuleFor(x => x.ActivityName.En)
                .NotEmpty().WithMessage("Activity name is required")
                .MaximumLength(55).WithMessage("Activity name cannot exceed 55 characters");

            RuleFor(x => x.ActivityName.Ar)
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
