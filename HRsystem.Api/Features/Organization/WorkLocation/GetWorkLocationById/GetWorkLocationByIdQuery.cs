using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;

namespace HRsystem.Api.Features.Organization.WorkLocation.GetWorkLocationById
{
    public record GetWorkLocationByIdQuery(int WorkLocationId) : IRequest<TbWorkLocation?>;

    public class Handler : IRequestHandler<GetWorkLocationByIdQuery, TbWorkLocation?>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbWorkLocation?> Handle(GetWorkLocationByIdQuery request, CancellationToken ct)
            => await _db.TbWorkLocations.FindAsync(new object[] { request.WorkLocationId }, ct);
    }
}
