using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Holiday.GetAllHolidays;
using HRsystem.Api.Shared.DTO;
using MediatR;
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

    public class CreateHolidayHandler : IRequestHandler<CreateHolidayCommand, HolidayDto>
    {
        private readonly DBContextHRsystem _db;
        public CreateHolidayHandler(DBContextHRsystem db) => _db = db;

        public async Task<HolidayDto> Handle(CreateHolidayCommand request, CancellationToken ct)
        {
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


            var hType = _db.TbHolidayTypes.Where(c => c.HolidayTypeId == entity.HolidayTypeId).Select(d => d.HolidayTypeName).FirstOrDefault();


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
                 HolidayTypeName= hType
            };
        }
    }

}
