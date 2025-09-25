using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Lookups.ActivityStatus.DeleteActivityStatus
{

    // Delete
    public record DeleteActivityStatusCommand(int StatusId) : IRequest<bool>;

    public class DeleteHandler : IRequestHandler<DeleteActivityStatusCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public DeleteHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteActivityStatusCommand request, CancellationToken ct)
        {
            var entity = await _db.TbActivityStatuses.FirstOrDefaultAsync(x => x.StatusId == request.StatusId, ct);
            if (entity == null) return false;

            _db.TbActivityStatuses.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
