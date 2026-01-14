using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Lookups.VacationType.DeleteVacationType
{
    public record DeleteVacationTypeCommand(int VacationTypeId) : IRequest<bool>;

    public class Handler : IRequestHandler<DeleteVacationTypeCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteVacationTypeCommand request, CancellationToken ct)
        {
            var entity = await _db.TbVacationTypes
                .FirstOrDefaultAsync(x => x.VacationTypeId == request.VacationTypeId, ct);

            if (entity == null) return false;

            _db.TbVacationTypes.Remove(entity);
            await _db.SaveChangesAsync(ct);

            return true;
        }
    }
}
