using MediatR;
using HRsystem.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Gov.DeleteGov
{
    public record DeleteGovCommand(int Id) : IRequest<bool>;

    public class Handler : IRequestHandler<DeleteGovCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteGovCommand request, CancellationToken ct)
        {
            var entity = await _db.TbGovs.FirstOrDefaultAsync(g => g.GovId == request.Id, ct);
            if (entity == null) return false;

            _db.TbGovs.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
