using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Features.Scheduling.Shift.UpdateShift;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRsystem.Api.Features.Scheduling.Shift.UpdateShift
{
    public record UpdateShiftCommand(
        int ShiftId,
        LocalizedData ShiftName,
        TimeOnly StartTime,
        TimeOnly EndTime,
        bool IsFlexible,
        TimeOnly? MinStartTime,
        TimeOnly? MaxStartTime,
        int GracePeriodMinutes,
        decimal? RequiredWorkingHours,
        string? Notes,
        int CompanyId
    ) : IRequest<bool>;

    public class UpdateShiftHandler : IRequestHandler<UpdateShiftCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public UpdateShiftHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(UpdateShiftCommand request, CancellationToken ct)
        {
            var shift = await _db.TbShifts.FirstOrDefaultAsync(s => s.ShiftId == request.ShiftId, ct);
            if (shift == null) return false;

            shift.ShiftName = request.ShiftName;
            shift.StartTime = request.StartTime;
            shift.EndTime = request.EndTime;
            shift.IsFlexible = request.IsFlexible;
            shift.MinStartTime = request.MinStartTime;
            shift.MaxStartTime = request.MaxStartTime;
            shift.GracePeriodMinutes = request.GracePeriodMinutes;
            shift.RequiredWorkingHours = request.RequiredWorkingHours;
            shift.Notes = request.Notes;
            shift.CompanyId = request.CompanyId;
            shift.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
            return true;
        }
    }

}

public class UpdateShiftValidator : AbstractValidator<UpdateShiftCommand>
{
    public UpdateShiftValidator()
    {
        RuleFor(x => x.ShiftId).GreaterThan(0);
        RuleFor(x => x.ShiftName).NotEmpty();
        RuleFor(x => x.StartTime).LessThan(x => x.EndTime);
    }
}
