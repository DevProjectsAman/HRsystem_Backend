using HRsystem.Api.Database;
using HRsystem.Api.Features.RemoteWorkDays;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.RemoteWorkdays.GetRemoteWorkDaysById
{

    public record GetRemoteWorkDaysByIdQuery(int Id) : IRequest<RemoteWorkDaysDto>;
    public class GetRemoteWorkDaysByIdHandler
        : IRequestHandler<GetRemoteWorkDaysByIdQuery, RemoteWorkDaysDto?>
    {
        private readonly DBContextHRsystem _db;

        public GetRemoteWorkDaysByIdHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<RemoteWorkDaysDto?> Handle(GetRemoteWorkDaysByIdQuery request, CancellationToken ct)
        {
            return await _db.TbRemoteWorkDays
                .Where(r => r.RemoteWorkDaysId == request.Id)
                .Select(r => new RemoteWorkDaysDto
                {
                    RemoteWorkDaysId = r.RemoteWorkDaysId,
                    RemoteWorkDaysNames = r.RemoteWorkDaysNames,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}
