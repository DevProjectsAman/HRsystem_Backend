using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.ActivityStatus.GetActivityStatusById
{

    // Get By Id
    public record GetActivityStatusByIdQuery(int StatusId) : IRequest<TbActivityStatus?>;

    public class GetByIdHandler : IRequestHandler<GetActivityStatusByIdQuery, TbActivityStatus?>
    {
        private readonly DBContextHRsystem _db;
        public GetByIdHandler(DBContextHRsystem db) => _db = db;

        public async Task<TbActivityStatus?> Handle(GetActivityStatusByIdQuery request, CancellationToken ct)
            => await _db.TbActivityStatuses.FirstOrDefaultAsync(x => x.StatusId == request.StatusId, ct);
    }

}
