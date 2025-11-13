using HRsystem.Api.Database;
using MediatR;
using System;

namespace HRsystem.Api.Features.Scheduling.RemoteWorkdays.UpdateRemoteWorkDays
{
    public record UpdateRemoteWorkDaysCommand(int Id, List<string> RemoteWorkDaysNames) : IRequest<bool>;

    public class UpdateRemoteWorkDaysHandler : IRequestHandler<UpdateRemoteWorkDaysCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public UpdateRemoteWorkDaysHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(UpdateRemoteWorkDaysCommand request, CancellationToken ct)
        {
            var entity = await _db.TbRemoteWorkDays.FindAsync(new object[] { request.Id }, ct);
            if (entity == null) return false;

            entity.RemoteWorkDaysNames = request.RemoteWorkDaysNames;
            entity.CreatedAt = DateTime.Now;

            _db.TbRemoteWorkDays.Update(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }

}
