using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using System;

namespace HRsystem.Api.Features.Scheduling.RemoteWorkdays.CreateRemoteWorkday
{
    public record CreateRemoteWorkDaysCommand(List<string> RemoteWorkDaysNames, int? CreatedBy) : IRequest<int>;

    public class CreateRemoteWorkDaysHandler : IRequestHandler<CreateRemoteWorkDaysCommand, int>
    {
        private readonly DBContextHRsystem _db;
        public CreateRemoteWorkDaysHandler(DBContextHRsystem db) => _db = db;

        public async Task<int> Handle(CreateRemoteWorkDaysCommand request, CancellationToken ct)
        {
            var entity = new TbRemoteWorkDay
            {
                RemoteWorkDaysNames = request.RemoteWorkDaysNames,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            _db.TbRemoteWorkDays.Add(entity);
            await _db.SaveChangesAsync(ct);

            return entity.RemoteWorkDaysId;
        }
    }


}
