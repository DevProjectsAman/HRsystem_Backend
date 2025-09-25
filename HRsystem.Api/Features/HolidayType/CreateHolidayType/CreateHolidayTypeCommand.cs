using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Shared.DTO;
using MediatR;

namespace HRsystem.Api.Features.HolidayType.CreateHolidayType
{
    public class CreateHolidayTypeDto
    {
        public LocalizedData HolidayTypeName { get; set; } = new LocalizedData();
    }
    // 🔹 Create
    public record CreateHolidayTypeCommand(CreateHolidayTypeDto Dto) : IRequest<int>;

    public class CreateHolidayTypeHandler : IRequestHandler<CreateHolidayTypeCommand, int>
    {
        private readonly DBContextHRsystem _db;
        public CreateHolidayTypeHandler(DBContextHRsystem db) => _db = db;

        public async Task<int> Handle(CreateHolidayTypeCommand request, CancellationToken ct)
        {
            var entity = new TbHolidayType
            {
                HolidayTypeName = request.Dto.HolidayTypeName
            };
            _db.TbHolidayTypes.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity.HolidayTypeId;
        }
    }
}
