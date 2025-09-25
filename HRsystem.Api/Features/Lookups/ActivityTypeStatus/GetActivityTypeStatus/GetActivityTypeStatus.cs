using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Lookups.ActivityTypeStatus.GetActivityTypeStatus
{

    public record GetAllActivityTypeStatusQuery() : IRequest<List<TbActivityTypeStatus>>;

    public class GetAllActivityTypeStatusHandler : IRequestHandler<GetAllActivityTypeStatusQuery, List<TbActivityTypeStatus>>
    {
        private readonly DBContextHRsystem _db;

        public GetAllActivityTypeStatusHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<List<TbActivityTypeStatus>> Handle(GetAllActivityTypeStatusQuery request, CancellationToken ct)
        {
            return await _db.TbActivityTypeStatuses.ToListAsync(ct);
        }
    }


    public record GetActivityTypeStatusByIdQuery(int Id) : IRequest<TbActivityTypeStatus?>;

    public class GetActivityTypeStatusByIdHandler : IRequestHandler<GetActivityTypeStatusByIdQuery, TbActivityTypeStatus?>
    {
        private readonly DBContextHRsystem _db;

        public GetActivityTypeStatusByIdHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<TbActivityTypeStatus?> Handle(GetActivityTypeStatusByIdQuery request, CancellationToken ct)
        {
            return await _db.TbActivityTypeStatuses
                            .FirstOrDefaultAsync(x => x.ActivityTypeStatusId == request.Id, ct);
        }
    }

}
