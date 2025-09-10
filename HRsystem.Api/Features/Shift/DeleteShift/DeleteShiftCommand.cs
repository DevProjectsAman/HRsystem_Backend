using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRsystem.Api.Features.Shift.DeleteShift
{
    public record DeleteShiftCommand(int ShiftId) : IRequest<bool>;

    public class DeleteShiftHandler : IRequestHandler<DeleteShiftCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public DeleteShiftHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteShiftCommand request, CancellationToken ct)
        {
            var shift = await _db.TbShifts.FirstOrDefaultAsync(s => s.ShiftId == request.ShiftId, ct);
            if (shift == null) return false;

            _db.TbShifts.Remove(shift);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
