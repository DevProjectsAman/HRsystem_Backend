using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;

public record CreateActivityTypeStatusCommand(
    int ActivityTypeId,
    int StatusId,
    bool IsDefault,
    bool IsActive,
    int CompanyId
) : IRequest<TbActivityTypeStatus>;

public class CreateActivityTypeStatusHandler : IRequestHandler<CreateActivityTypeStatusCommand, TbActivityTypeStatus>
{
    private readonly DBContextHRsystem _db;

    public CreateActivityTypeStatusHandler(DBContextHRsystem db)
    {
        _db = db;
    }

    public async Task<TbActivityTypeStatus> Handle(CreateActivityTypeStatusCommand request, CancellationToken ct)
    {
        var entity = new TbActivityTypeStatus
        {
            ActivityTypeId = request.ActivityTypeId,
            StatusId = request.StatusId,
            IsDefault = request.IsDefault,
            IsActive = request.IsActive,
            CompanyId = request.CompanyId
        };

        _db.TbActivityTypeStatuses.Add(entity);
        await _db.SaveChangesAsync(ct);

        return entity;
    }
}
