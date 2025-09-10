using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.ActivityStatus.UpdateActivityStatus;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.ActivityStatus.UpdateActivityStatus
{
    public record UpdateActivityStatusCommand(
        int StatusId,
        string StatusCode,
        string StatusName,
        bool IsFinal,
        int? CompanyId
    ) : IRequest<TbActivityStatus?>;

    public class Handler : IRequestHandler<UpdateActivityStatusCommand, TbActivityStatus?>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbActivityStatus?> Handle(UpdateActivityStatusCommand request, CancellationToken ct)
        {
            var entity = await _db.TbActivityStatuses.FirstOrDefaultAsync(x => x.StatusId == request.StatusId, ct);
            if (entity == null) return null;

            entity.StatusCode = request.StatusCode;
            entity.StatusName = request.StatusName;
            entity.IsFinal = request.IsFinal;
            entity.CompanyId = request.CompanyId;

            await _db.SaveChangesAsync(ct);
            return entity;
        }
    }
}

public class UpdateActivityStatusValidator : AbstractValidator<UpdateActivityStatusCommand>
{
    public UpdateActivityStatusValidator()
    {
        RuleFor(x => x.StatusId).GreaterThan(0);
        RuleFor(x => x.StatusCode).NotEmpty().MaximumLength(25);
        RuleFor(x => x.StatusName).NotEmpty().MaximumLength(55);
    }
}
