using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRsystem.Api.Features.Scheduling.Shift.GetShiftByIdQuery
{
    public record GetShiftByIdQuery(int ShiftId) : IRequest<TbShift?>;

    public class GetShiftByIdHandler : IRequestHandler<GetShiftByIdQuery, TbShift?>
    {
        private readonly DBContextHRsystem _db;
        public GetShiftByIdHandler(DBContextHRsystem db) => _db = db;

        public async Task<TbShift?> Handle(GetShiftByIdQuery request, CancellationToken ct)
        {
            return await _db.TbShifts.FirstOrDefaultAsync(s => s.ShiftId == request.ShiftId, ct);
        }
    }
}
