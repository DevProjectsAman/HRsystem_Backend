using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Lookups.ActivityStatus.CreateActivityStatus;
using HRsystem.Api.Shared.DTO;
using MediatR;

namespace HRsystem.Api.Features.Lookups.ActivityStatus.CreateActivityStatus
{
    public record CreateActivityStatusCommand(
        string StatusCode,
        LocalizedData StatusName,
        bool IsFinal,
        int? CreatedBy,
        int? CompanyId
    ) : IRequest<TbActivityStatus>;

    public class Handler : IRequestHandler<CreateActivityStatusCommand, TbActivityStatus>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbActivityStatus> Handle(CreateActivityStatusCommand request, CancellationToken ct)
        {
            var entity = new TbActivityStatus
            {
                StatusCode = request.StatusCode,
                StatusName = request.StatusName,
                IsFinal = request.IsFinal,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.CreatedBy,
                CompanyId = request.CompanyId
            };

            _db.TbActivityStatuses.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }
    }


}

public class CreateActivityStatusValidator : AbstractValidator<CreateActivityStatusCommand>
{
    public CreateActivityStatusValidator()
    {
        RuleFor(x => x.StatusCode).NotEmpty().MaximumLength(25);
        RuleFor(x => x.StatusName.en).NotEmpty().MaximumLength(55);
        RuleFor(x => x.StatusName.ar).NotEmpty().MaximumLength(55);
    }
}