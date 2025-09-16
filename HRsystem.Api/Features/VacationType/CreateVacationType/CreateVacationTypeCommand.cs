using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Shared.DTO;
using MediatR;

namespace HRsystem.Api.Features.VacationType.CreateVacationType
{
    public record CreateVacationTypeCommand(
        LocalizedData VacationName,
        string? Description,
        bool? IsPaid,
        bool? RequiresHrApproval
    ) : IRequest<TbVacationType>;

    public class Handler : IRequestHandler<CreateVacationTypeCommand, TbVacationType>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbVacationType> Handle(CreateVacationTypeCommand request, CancellationToken ct)
        {
            var entity = new TbVacationType
            {
                VacationName = request.VacationName,
                Description = request.Description,
                IsPaid = request.IsPaid,
                RequiresHrApproval = request.RequiresHrApproval
            };

            _db.TbVacationTypes.Add(entity);
            await _db.SaveChangesAsync(ct);

            return entity;
        }
    }
}


namespace HRsystem.Api.Features.VacationType.CreateVacationType
{
    public class CreateVacationTypeValidator : AbstractValidator<CreateVacationTypeCommand>
    {
        public CreateVacationTypeValidator()
        {
            RuleFor(x => x.VacationName.en)
                .NotEmpty().WithMessage("VacationName is required")
                .MaximumLength(100).WithMessage("VacationName must not exceed 100 characters");

            RuleFor(x => x.VacationName.ar)
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