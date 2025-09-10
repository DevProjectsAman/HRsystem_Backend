using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.ActivityStatus.GetAllActivityStatuses
{
    // Get All
    public record GetAllActivityStatusesQuery() : IRequest<List<TbActivityStatus>>;

    public class GetAllHandler : IRequestHandler<GetAllActivityStatusesQuery, List<TbActivityStatus>>
    {
        private readonly DBContextHRsystem _db;
        public GetAllHandler(DBContextHRsystem db) => _db = db;

        public async Task<List<TbActivityStatus>> Handle(GetAllActivityStatusesQuery request, CancellationToken ct)
            => await _db.TbActivityStatuses.ToListAsync(ct);
    }

}
