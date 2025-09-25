using HRsystem.Api.Database;
using HRsystem.Api.Features.Holiday.GetAllHolidays;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRsystem.Api.Features.Holiday.GetHolidayById
{
    public record GetHolidayByIdQuery(int Id) : IRequest<HolidayDto?>;

    public class GetHolidayByIdHandler : IRequestHandler<GetHolidayByIdQuery, HolidayDto?>
    {
        private readonly DBContextHRsystem _db;
        public GetHolidayByIdHandler(DBContextHRsystem db) => _db = db;

        public async Task<HolidayDto?> Handle(GetHolidayByIdQuery request, CancellationToken ct)
        {
            return await _db.TbHolidays
                .Include(h => h.HolidayType)
                .Where(h => h.HolidayId == request.Id)
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
                .FirstOrDefaultAsync(ct);
        }
    }

}
