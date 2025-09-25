using HRsystem.Api.Database;
using MediatR;
using System;

namespace HRsystem.Api.Features.Scheduling.RemoteWorkdays.DeleteRemoteWorkDays
{
    public record DeleteRemoteWorkDaysCommand(int Id) : IRequest<bool>;

    public class DeleteRemoteWorkDaysHandler : IRequestHandler<DeleteRemoteWorkDaysCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public DeleteRemoteWorkDaysHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteRemoteWorkDaysCommand request, CancellationToken ct)
        {
            var entity = await _db.TbRemoteWorkDays.FindAsync(new object[] { request.Id }, ct);
            if (entity == null) return false;

            _db.TbRemoteWorkDays.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }

}
