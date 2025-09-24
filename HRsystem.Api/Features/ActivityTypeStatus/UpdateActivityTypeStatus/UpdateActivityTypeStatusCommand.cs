using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

public record UpdateActivityTypeStatusCommand(
    int ActivityTypeStatusId,
    int ActivityTypeId,
    int StatusId,
    bool IsDefault,
    bool IsActive,
    int CompanyId
) : IRequest<TbActivityTypeStatus?>;

public class UpdateActivityTypeStatusHandler : IRequestHandler<UpdateActivityTypeStatusCommand, TbActivityTypeStatus?>
{
    private readonly DBContextHRsystem _db;

    public UpdateActivityTypeStatusHandler(DBContextHRsystem db)
    {
        _db = db;
    }

    public async Task<TbActivityTypeStatus?> Handle(UpdateActivityTypeStatusCommand request, CancellationToken ct)
    {
        var entity = await _db.TbActivityTypeStatuses
                              .FirstOrDefaultAsync(x => x.ActivityTypeStatusId == request.ActivityTypeStatusId, ct);

        if (entity == null) return null;

        entity.ActivityTypeId = request.ActivityTypeId;
        entity.StatusId = request.StatusId;
        entity.IsDefault = request.IsDefault;
        entity.IsActive = request.IsActive;
        entity.CompanyId = request.CompanyId;

        await _db.SaveChangesAsync(ct);

        return entity;
    }
}
