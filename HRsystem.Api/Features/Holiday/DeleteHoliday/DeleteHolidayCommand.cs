using HRsystem.Api.Database;
using MediatR;
using System;

namespace HRsystem.Api.Features.Holiday.DeleteHoliday
{
    public record DeleteHolidayCommand(int Id) : IRequest<bool>;

    public class DeleteHolidayHandler : IRequestHandler<DeleteHolidayCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public DeleteHolidayHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteHolidayCommand request, CancellationToken ct)
        {
            var entity = await _db.TbHolidays.FindAsync(new object[] { request.Id }, ct);
            if (entity == null) return false;

            _db.TbHolidays.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }

}
