using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Holiday.GetAllHolidays;
using HRsystem.Api.Shared.DTO;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRsystem.Api.Features.Holiday.CreateHoliday
{
    public record CreateHolidayCommand(
        int HolidayTypeId,
        LocalizedData HolidayName,
        DateTime StartDate,
        DateTime EndDate,
        bool IsForChristiansOnly,
        bool IsActive,
        int? CompanyId
    ) : IRequest<HolidayDto>;

    // Request validator: basic shape rules (runs in validation pipeline)
    public class CreateHolidayCommandValidator : AbstractValidator<CreateHolidayCommand>
    {
        public CreateHolidayCommandValidator()
        {
            RuleFor(x => x.HolidayTypeId).GreaterThan(0);
            RuleFor(x => x.HolidayName).NotNull().WithMessage("HolidayName is required.");
            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate)
                .WithMessage("StartDate must be on or before EndDate.");
        }
    }

    public class CreateHolidayHandler : IRequestHandler<CreateHolidayCommand, HolidayDto>
    {
        private readonly DBContextHRsystem _db;
        public CreateHolidayHandler(DBContextHRsystem db) => _db = db;

        public async Task<HolidayDto> Handle(CreateHolidayCommand request, CancellationToken ct)
        {
            // DB-level validations with descriptive failures
            var failures = new List<ValidationFailure>();

            // Validate HolidayType exists
            var holidayTypeExists = await _db.TbHolidayTypes
                .AnyAsync(ht => ht.HolidayTypeId == request.HolidayTypeId, ct);
            if (!holidayTypeExists)
                failures.Add(new ValidationFailure(nameof(request.HolidayTypeId), $"HolidayTypeId {request.HolidayTypeId} not found."));

            // Validate date range (defensive - validator already checks this but double-check here)
            if (request.StartDate > request.EndDate)
                failures.Add(new ValidationFailure("DateRange", "StartDate must be on or before EndDate."));

            // If CompanyId is provided, ensure no overlapping holiday exists for same company
            if (request.CompanyId.HasValue)
            {
                var overlap = await _db.TbHolidays.AnyAsync(h =>
                    h.CompanyId == request.CompanyId
                    && h.StartDate <= request.EndDate
                    && h.EndDate >= request.StartDate, ct);

                if (overlap)
                    failures.Add(new ValidationFailure("DateOverlap", "A holiday already exists in the given date range for this company."));
            }

            if (failures.Any())
                throw new ValidationException(failures);

            var entity = new TbHolidays
            {
                HolidayTypeId = request.HolidayTypeId,
                HolidayName = request.HolidayName,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsForChristiansOnly = request.IsForChristiansOnly,
                IsActive = request.IsActive,
                CompanyId = request.CompanyId
            };

            _db.TbHolidays.Add(entity);
            await _db.SaveChangesAsync(ct);

            var hType = await _db.TbHolidayTypes
                .Where(c => c.HolidayTypeId == entity.HolidayTypeId)
                .Select(d => d.HolidayTypeName)
                .FirstOrDefaultAsync(ct);

            return new HolidayDto
            {
                HolidayId = entity.HolidayId,
                HolidayTypeId = entity.HolidayTypeId,
                HolidayName = entity.HolidayName,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsForChristiansOnly = entity.IsForChristiansOnly,
                IsActive = entity.IsActive,
                CompanyId = entity.CompanyId,
                HolidayTypeName = hType
            };
        }
    }
}
