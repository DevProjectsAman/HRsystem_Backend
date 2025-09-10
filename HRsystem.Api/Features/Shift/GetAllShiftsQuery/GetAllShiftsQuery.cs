using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Shift.GetAllShifts
{
    
    public record GetAllShiftsQuery() : IRequest<List<TbShift>>;

    public class GetAllShiftsHandler : IRequestHandler<GetAllShiftsQuery, List<TbShift>>
    {
        private readonly DBContextHRsystem _db;
        public GetAllShiftsHandler(DBContextHRsystem db) => _db = db;

        public async Task<List<TbShift>> Handle(GetAllShiftsQuery request, CancellationToken ct)
        {
            return await _db.TbShifts.ToListAsync(ct);
        }
    }
}
