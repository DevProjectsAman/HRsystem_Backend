using HRsystem.Api.Database;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.HolidayType.GetAllHolidayTypes
{
    // 🔹 Get All

    public class HolidayTypeDto
    {
        public int HolidayTypeId { get; set; }
        public LocalizedData HolidayTypeName { get; set; } = new LocalizedData();
    }
    public record GetAllHolidayTypesQuery() : IRequest<List<HolidayTypeDto>>;

    public class GetAllHolidayTypesHandler : IRequestHandler<GetAllHolidayTypesQuery, List<HolidayTypeDto>>
    {
        private readonly DBContextHRsystem _db;
        public GetAllHolidayTypesHandler(DBContextHRsystem db) => _db = db;

        public async Task<List<HolidayTypeDto>> Handle(GetAllHolidayTypesQuery request, CancellationToken ct)
        {
            return await _db.TbHolidayTypes
                .Select(h => new HolidayTypeDto
                {
                    HolidayTypeId = h.HolidayTypeId,
                    HolidayTypeName = h.HolidayTypeName
                })
                .ToListAsync(ct);
        }
    }
}
