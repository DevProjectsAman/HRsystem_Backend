using HRsystem.Api.Database;
using HRsystem.Api.Shared.DTO;
using MediatR;

namespace HRsystem.Api.Features.HolidayType.UpdateHolidayType
{

    public class UpdateHolidayTypeDto
    {
        public int HolidayTypeId { get; set; }
        public LocalizedData HolidayTypeName { get; set; } = new LocalizedData();
    }
    // 🔹 Update
    public record UpdateHolidayTypeCommand(UpdateHolidayTypeDto Dto) : IRequest<bool>;

    public class UpdateHolidayTypeHandler : IRequestHandler<UpdateHolidayTypeCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public UpdateHolidayTypeHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(UpdateHolidayTypeCommand request, CancellationToken ct)
        {
            var entity = await _db.TbHolidayTypes.FindAsync(new object[] { request.Dto.HolidayTypeId }, ct);
            if (entity == null) return false;

            entity.HolidayTypeName = request.Dto.HolidayTypeName;

            _db.TbHolidayTypes.Update(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
