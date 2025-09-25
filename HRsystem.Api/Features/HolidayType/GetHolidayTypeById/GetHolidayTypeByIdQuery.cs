using HRsystem.Api.Database;
using HRsystem.Api.Features.HolidayType.GetAllHolidayTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.HolidayType.GetHolidayTypeById
{
    public record GetHolidayTypeByIdQuery(int Id) : IRequest<HolidayTypeDto?>;

    public class GetHolidayTypeByIdHandler : IRequestHandler<GetHolidayTypeByIdQuery, HolidayTypeDto?>
    {
        private readonly DBContextHRsystem _db;
        public GetHolidayTypeByIdHandler(DBContextHRsystem db) => _db = db;

        public async Task<HolidayTypeDto?> Handle(GetHolidayTypeByIdQuery request, CancellationToken ct)
        {
            return await _db.TbHolidayTypes
                .Where(h => h.HolidayTypeId == request.Id)
                .Select(h => new HolidayTypeDto
                {
                    HolidayTypeId = h.HolidayTypeId,
                    HolidayTypeName = h.HolidayTypeName
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}
