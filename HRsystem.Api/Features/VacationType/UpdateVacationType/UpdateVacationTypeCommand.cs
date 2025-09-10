using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.VacationType.UpdateVacationType
{
    public record UpdateVacationTypeCommand(
        int VacationTypeId,
        string VacationName,
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
