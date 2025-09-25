using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.ActivityType.GetActivityTypeById
{
    public record GetActivityTypeByIdQuery(int ActivityTypeId) : IRequest<TbActivityType?>;

    public class Handler : IRequestHandler<GetActivityTypeByIdQuery, TbActivityType?>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbActivityType?> Handle(GetActivityTypeByIdQuery request, CancellationToken ct)
        {
            return await _db.TbActivityTypes.FirstOrDefaultAsync(x => x.ActivityTypeId == request.ActivityTypeId, ct);
        }
    }
}
