using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.Govermenet.UpdateGov
{
    public record UpdateGovCommand(int GovId, string? GoveCode, string? GovName, string? GovArea) : IRequest<TbGov?>;

    public class Handler : IRequestHandler<UpdateGovCommand, TbGov?>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbGov?> Handle(UpdateGovCommand request, CancellationToken ct)
        {
            var entity = await _db.TbGovs.FirstOrDefaultAsync(g => g.GovId == request.GovId, ct);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Gov with ID {request.GovId} not found.");
            }

            entity.GoveCode = request.GoveCode;
            entity.GovName = request.GovName;
            entity.GovArea = request.GovArea;

            await _db.SaveChangesAsync(ct);
            return entity;
        }
    }
}


namespace HRsystem.Api.Features.Organization.Govermenet.UpdateGov
{
    public class UpdateGovValidator : AbstractValidator<UpdateGovCommand>
    {
        public UpdateGovValidator()
        {
            RuleFor(x => x.GovId).GreaterThan(0);
            RuleFor(x => x.GovName).NotEmpty().MaximumLength(60);
            RuleFor(x => x.GoveCode).MaximumLength(25);
            RuleFor(x => x.GovArea).MaximumLength(100);
        }
    }
}
