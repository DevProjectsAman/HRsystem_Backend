using HRsystem.Api.Database;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRsystem.Api.Features.Holiday.GetAllHolidays
{
    public record GetAllHolidaysQuery() : IRequest<List<HolidayDto>>;

    public class HolidayDto
    {
        public int HolidayId { get; set; }
        public int HolidayTypeId { get; set; }
        public string HolidayTypeName { get; set; } = "";
        public LocalizedData HolidayName { get; set; } = new LocalizedData();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsForChristiansOnly { get; set; }
        public bool IsActive { get; set; }
        public int? CompanyId { get; set; }
    }

    public class GetAllHolidaysHandler : IRequestHandler<GetAllHolidaysQuery, List<HolidayDto>>
    {
        private readonly DBContextHRsystem _db;
        public GetAllHolidaysHandler(DBContextHRsystem db) => _db = db;

        public async Task<List<HolidayDto>> Handle(GetAllHolidaysQuery request, CancellationToken ct)
        {
            return await _db.TbHolidays
                .Include(h => h.HolidayType)
                .Select(h => new HolidayDto
                {
                    HolidayId = h.HolidayId,
                    HolidayTypeId = h.HolidayTypeId,
                    HolidayTypeName = h.HolidayType.HolidayTypeName.en,
                    HolidayName = h.HolidayName,
                    StartDate = h.StartDate,
                    EndDate = h.EndDate,
                    IsForChristiansOnly = h.IsForChristiansOnly,
                    IsActive = h.IsActive,
                    CompanyId = h.CompanyId
                })
                .ToListAsync(ct);
        }
    }

}
