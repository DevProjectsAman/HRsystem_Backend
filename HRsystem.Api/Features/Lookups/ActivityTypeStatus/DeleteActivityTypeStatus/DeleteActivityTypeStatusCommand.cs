using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

public record DeleteActivityTypeStatusCommand(int Id) : IRequest<TbActivityTypeStatus?>;

public class DeleteActivityTypeStatusHandler : IRequestHandler<DeleteActivityTypeStatusCommand, TbActivityTypeStatus?>
{
    private readonly DBContextHRsystem _db;

    public DeleteActivityTypeStatusHandler(DBContextHRsystem db)
    {
        _db = db;
    }

    public async Task<TbActivityTypeStatus?> Handle(DeleteActivityTypeStatusCommand request, CancellationToken ct)
    {
        var entity = await _db.TbActivityTypeStatuses
                              .FirstOrDefaultAsync(x => x.ActivityTypeStatusId == request.Id, ct);

        if (entity == null) return null;

        _db.TbActivityTypeStatuses.Remove(entity);
        await _db.SaveChangesAsync(ct);

        return entity;
    }
}
