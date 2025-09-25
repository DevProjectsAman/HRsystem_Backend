using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRsystem.Api.Features.Scheduling.RemoteWorkdays.GetAllRemoteWorkdays
{
    public record GetAllRemoteWorkDaysQuery : IRequest<List<RemoteWorkDaysDto>>;

    public class RemoteWorkDaysDto
    {
        public int RemoteWorkDaysId { get; set; }
        public List<string> RemoteWorkDaysNames { get; set; } = new();
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
    public class GetAllRemoteWorkDaysHandler : IRequestHandler<GetAllRemoteWorkDaysQuery, List<RemoteWorkDaysDto>>
    {
        private readonly DBContextHRsystem _db;
        public GetAllRemoteWorkDaysHandler(DBContextHRsystem db) => _db = db;

        public async Task<List<RemoteWorkDaysDto>> Handle(GetAllRemoteWorkDaysQuery request, CancellationToken ct)
        {
            return await _db.TbRemoteWorkDays
                .Select(r => new RemoteWorkDaysDto
                {
                    RemoteWorkDaysId = r.RemoteWorkDaysId,
                    RemoteWorkDaysNames = r.RemoteWorkDaysNames,
                    CreatedBy = r.CreatedBy,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(ct);
        }
    }
}
