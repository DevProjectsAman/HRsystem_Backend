using HRsystem.Api.Database.DataTables;
using MediatR;
using HRsystem.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Gov.GetGovById
{
    public record GetGovByIdQuery(int Id) : IRequest<TbGov?>;

    public class Handler : IRequestHandler<GetGovByIdQuery, TbGov?>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbGov?> Handle(GetGovByIdQuery request, CancellationToken ct)
        {
            return await _db.TbGovs.FirstOrDefaultAsync(g => g.GovId == request.Id, ct);
        }
    }
}
