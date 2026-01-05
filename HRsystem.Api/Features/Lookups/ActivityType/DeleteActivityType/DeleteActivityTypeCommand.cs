using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Lookups.ActivityType.DeleteActivityType
{
    public record DeleteActivityTypeCommand(int ActivityTypeId) : IRequest<bool>;

    public class Handler : IRequestHandler<DeleteActivityTypeCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteActivityTypeCommand request, CancellationToken ct)
        {
            var entity = await _db.TbActivityTypes.FirstOrDefaultAsync(x => x.ActivityTypeId == request.ActivityTypeId, ct);
            if (entity == null) return false;

            _db.TbActivityTypes.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
