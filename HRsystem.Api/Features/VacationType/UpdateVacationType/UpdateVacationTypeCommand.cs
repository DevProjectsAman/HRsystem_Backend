using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.VacationType.UpdateVacationType;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.VacationType.UpdateVacationType
{
    public record UpdateVacationTypeCommand(
        int VacationTypeId,
        LocalizedData VacationName,
        string? Description,
        bool? IsPaid,
        bool? RequiresHrApproval
    ) : IRequest<TbVacationType?>;

    public class Handler : IRequestHandler<UpdateVacationTypeCommand, TbVacationType?>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbVacationType?> Handle(UpdateVacationTypeCommand request, CancellationToken ct)
        {
            var entity = await _db.TbVacationTypes
                .FirstOrDefaultAsync(x => x.VacationTypeId == request.VacationTypeId, ct);

            if (entity == null) return null;

            entity.VacationName = request.VacationName;
            entity.Description = request.Description;
            entity.IsPaid = request.IsPaid;
            entity.RequiresHrApproval = request.RequiresHrApproval;

            await _db.SaveChangesAsync(ct);
            return entity;
        }
    }
}
namespace HRsystem.Api.Features.VacationType.UpdateVacationType
{
    public class UpdateVacationTypeValidator : AbstractValidator<UpdateVacationTypeCommand>
    {
        public UpdateVacationTypeValidator()
        {
            RuleFor(x => x.VacationTypeId)
                .GreaterThan(0).WithMessage("VacationTypeId is required");

            RuleFor(x => x.VacationName.En)
                .NotEmpty().WithMessage("VacationName is required")
                .MaximumLength(100).WithMessage("VacationName must not exceed 100 characters");

            RuleFor(x => x.VacationName.Ar)
               .NotEmpty().WithMessage("VacationName is required")
               .MaximumLength(100).WithMessage("VacationName must not exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters");

            RuleFor(x => x.IsPaid)
                .NotNull().WithMessage("IsPaid must be specified (true/false)");

            RuleFor(x => x.RequiresHrApproval)
                .NotNull().WithMessage("RequiresHrApproval must be specified (true/false)");
        }
    }
}