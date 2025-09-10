using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.VacationType.GetAllVacationTypes
{
    public record GetAllVacationTypesQuery() : IRequest<List<TbVacationType>>;

    public class Handler : IRequestHandler<GetAllVacationTypesQuery, List<TbVacationType>>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<List<TbVacationType>> Handle(GetAllVacationTypesQuery request, CancellationToken ct)
            => await _db.TbVacationTypes.ToListAsync(ct);
    }
}
