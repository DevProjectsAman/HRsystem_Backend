using HRsystem.Api.Database;
using HRsystem.Api.Features.Holiday.GetAllHolidays;
using HRsystem.Api.Shared.DTO;
using MediatR;
using System;

namespace HRsystem.Api.Features.Holiday.UpdateHoliday
{
    public record UpdateHolidayCommand(
        int HolidayId,
        int HolidayTypeId,
        LocalizedData HolidayName,
        DateTime StartDate,
        DateTime EndDate,
        bool IsForChristiansOnly,
        bool IsActive,
        int? CompanyId
    ) : IRequest<HolidayDto?>;

    public class UpdateHolidayHandler : IRequestHandler<UpdateHolidayCommand, HolidayDto?>
    {
        private readonly DBContextHRsystem _db;
        public UpdateHolidayHandler(DBContextHRsystem db) => _db = db;

        public async Task<HolidayDto?> Handle(UpdateHolidayCommand request, CancellationToken ct)
        {
            var entity = await _db.TbHolidays.FindAsync(new object[] { request.HolidayId }, ct);
            if (entity == null) return null;

            entity.HolidayTypeId = request.HolidayTypeId;
            entity.HolidayName = request.HolidayName;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.IsForChristiansOnly = request.IsForChristiansOnly;
            entity.IsActive = request.IsActive;
            entity.CompanyId = request.CompanyId;

            await _db.SaveChangesAsync(ct);

            return new HolidayDto
            {
                HolidayId = entity.HolidayId,
                HolidayTypeId = entity.HolidayTypeId,
                HolidayName = entity.HolidayName,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsForChristiansOnly = entity.IsForChristiansOnly,
                IsActive = entity.IsActive,
                CompanyId = entity.CompanyId
            };
        }
    }

}
