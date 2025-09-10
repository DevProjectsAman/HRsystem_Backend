using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Shift;
using MediatR;
using System;

namespace HRsystem.Api.Features.Shift
{
    public record CreateShiftCommand(
    string ShiftName,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsFlexible,
    TimeOnly? MinStartTime,
    TimeOnly? MaxStartTime,
    int GracePeriodMinutes,
    decimal? RequiredWorkingHours,
    string? Notes,
    int CompanyId
) : IRequest<int>;


    public class CreateShiftHandler : IRequestHandler<CreateShiftCommand, int>
    {
        private readonly DBContextHRsystem _db;
        public CreateShiftHandler(DBContextHRsystem db) => _db = db;

        public async Task<int> Handle(CreateShiftCommand request, CancellationToken ct)
        {
            var shift = new TbShift
            {
                ShiftName = request.ShiftName,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsFlexible = request.IsFlexible,
                MinStartTime = request.MinStartTime,
                MaxStartTime = request.MaxStartTime,
                GracePeriodMinutes = request.GracePeriodMinutes,
                RequiredWorkingHours = request.RequiredWorkingHours,
                Notes = request.Notes,
                CompanyId = request.CompanyId,
                CreatedAt = DateTime.UtcNow
            };

            _db.TbShifts.Add(shift);
            await _db.SaveChangesAsync(ct);
            return shift.ShiftId;
        }
    }
}

public class CreateShiftValidator : AbstractValidator<CreateShiftCommand>
{
    public CreateShiftValidator()
    {
        RuleFor(x => x.ShiftName).NotEmpty().WithMessage("Shift name is required");
        RuleFor(x => x.StartTime).LessThan(x => x.EndTime).WithMessage("Start time must be before end time");
    }
}