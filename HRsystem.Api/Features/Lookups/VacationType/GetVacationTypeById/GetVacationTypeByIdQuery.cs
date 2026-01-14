using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;

namespace HRsystem.Api.Features.Lookups.VacationType.GetVacationTypeById
{
    public record GetVacationTypeByIdQuery(int VacationTypeId) : IRequest<TbVacationType?>;

    public class Handler : IRequestHandler<GetVacationTypeByIdQuery, TbVacationType?>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbVacationType?> Handle(GetVacationTypeByIdQuery request, CancellationToken ct)
            => await _db.TbVacationTypes.FindAsync(new object[] { request.VacationTypeId }, ct);
    }
}
