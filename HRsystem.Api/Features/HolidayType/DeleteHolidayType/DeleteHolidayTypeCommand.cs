using HRsystem.Api.Database;
using MediatR;

namespace HRsystem.Api.Features.HolidayType.DeleteHolidayType
{
    // 🔹 Delete
    public record DeleteHolidayTypeCommand(int Id) : IRequest<bool>;

    public class DeleteHolidayTypeHandler : IRequestHandler<DeleteHolidayTypeCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public DeleteHolidayTypeHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteHolidayTypeCommand request, CancellationToken ct)
        {
            var entity = await _db.TbHolidayTypes.FindAsync(new object[] { request.Id }, ct);
            if (entity == null) return false;

            _db.TbHolidayTypes.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
