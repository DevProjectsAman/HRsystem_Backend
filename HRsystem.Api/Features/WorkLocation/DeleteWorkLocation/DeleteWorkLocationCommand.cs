using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.WorkLocation.DeleteWorkLocation
{
    public record DeleteWorkLocationCommand(int WorkLocationId) : IRequest<bool>;

    public class Handler : IRequestHandler<DeleteWorkLocationCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteWorkLocationCommand request, CancellationToken ct)
        {
            var entity = await _db.TbWorkLocations
                .FirstOrDefaultAsync(x => x.WorkLocationId == request.WorkLocationId, ct);

            if (entity == null) return false;

            _db.TbWorkLocations.Remove(entity);
            await _db.SaveChangesAsync(ct);

            return true;
        }
    }
}
