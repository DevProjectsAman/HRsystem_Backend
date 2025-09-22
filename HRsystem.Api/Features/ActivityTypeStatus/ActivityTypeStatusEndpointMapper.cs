// File: Features/ActivityTypeStatus/ActivityTypeStatusEndpoints.cs
using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.ActivityTypeStatus
{
    #region Endpoints
    public static class ActivityTypeStatusEndpointMapper
    {
        public static void MapActivityTypeStatusEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/activity-type-status")
                           .WithTags("Activity Type Status");

            // CREATE
            group.MapPost("/AddNew", async (CreateActivityTypeStatusCommand cmd, IMediator mediator) =>
                await mediator.Send(cmd));

            // GET by Id
            group.MapGet("/GetById{id:int}", async (int id, IMediator mediator) =>
                await mediator.Send(new GetActivityTypeStatusByIdQuery(id)));

            // GET list by ActivityType + Company
            group.MapGet("/by-activity/GetBy{activityTypeId:int}/company/{companyId:int}", async (int activityTypeId, int companyId, IMediator mediator) =>
                await mediator.Send(new GetActivityTypeStatusListByTypeQuery(activityTypeId, companyId)));

            // UPDATE
            group.MapPut("/Update{id:int}", async (int id, UpdateActivityTypeStatusCommand cmd, IMediator mediator) =>
            {
                // enforce id from route
                cmd = cmd with { ActivityTypeStatusId = id };
                return await mediator.Send(cmd);
            });

            // DELETE
            group.MapDelete("/DeleteById{id:int}", async (int id, IMediator mediator) =>
                await mediator.Send(new DeleteActivityTypeStatusCommand(id)));
        }
    }

#endregion



#region DTOs
public record ActivityTypeStatusDto(
        int ActivityTypeStatusId,
        int ActivityTypeId,
        int StatusId,
        bool IsDefault,
        bool IsActive,
        int CompanyId
    );
#endregion

#region Create
public record CreateActivityTypeStatusCommand(
    int ActivityTypeId,
    int StatusId,
    bool IsDefault,
    bool IsActive,
    int CompanyId
) : IRequest<ResponseResultDTO<ActivityTypeStatusDto>>;

public class CreateActivityTypeStatusValidator : AbstractValidator<CreateActivityTypeStatusCommand>
{
    public CreateActivityTypeStatusValidator()
    {
        RuleFor(x => x.ActivityTypeId).GreaterThan(0);
        RuleFor(x => x.StatusId).GreaterThan(0);
        RuleFor(x => x.CompanyId).GreaterThan(0);
    }
}

public class CreateActivityTypeStatusHandler : IRequestHandler<CreateActivityTypeStatusCommand, ResponseResultDTO<ActivityTypeStatusDto>>
{
    private readonly DBContextHRsystem _db;
    private readonly ICurrentUserService _currentUser;

    public CreateActivityTypeStatusHandler(DBContextHRsystem db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<ResponseResultDTO<ActivityTypeStatusDto>> Handle(CreateActivityTypeStatusCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<ResponseErrorDTO>();

        // Validate foreign keys exist
        var existsActivityType = await _db.TbActivityTypes.AnyAsync(x => x.ActivityTypeId == request.ActivityTypeId && x.CompanyId == request.CompanyId, cancellationToken);
        if (!existsActivityType)
            errors.Add(new ResponseErrorDTO { Property = nameof(request.ActivityTypeId), Error = $"ActivityTypeId {request.ActivityTypeId} not found for company {request.CompanyId}." });

        var existsStatus = await _db.TbActivityStatuses.AnyAsync(x => x.StatusId == request.StatusId && (x.CompanyId == null || x.CompanyId == request.CompanyId), cancellationToken);
        if (!existsStatus)
            errors.Add(new ResponseErrorDTO { Property = nameof(request.StatusId), Error = $"StatusId {request.StatusId} not found or not available for company {request.CompanyId}." });

        if (errors.Any())
        {
            return new ResponseResultDTO<ActivityTypeStatusDto>
            {
                Success = false,
                Message = "Validation failed",
                Errors = errors
            };
        }

        // If IsDefault true, clear other defaults for same type/company
        if (request.IsDefault)
        {
            var others = await _db.TbActivityTypeStatuses
                .Where(x => x.ActivityTypeId == request.ActivityTypeId && x.CompanyId == request.CompanyId && x.IsDefault)
                .ToListAsync(cancellationToken);
            foreach (var o in others) o.IsDefault = false;
        }

        var entity = new TbActivityTypeStatus
        {
            ActivityTypeId = request.ActivityTypeId,
            StatusId = request.StatusId,
            IsDefault = request.IsDefault,
            IsActive = request.IsActive,
            CompanyId = request.CompanyId
        };

        _db.TbActivityTypeStatuses.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        var dto = new ActivityTypeStatusDto(
            entity.ActivityTypeStatusId,
            entity.ActivityTypeId,
            entity.StatusId,
            entity.IsDefault,
            entity.IsActive,
            entity.CompanyId
        );

        return new ResponseResultDTO<ActivityTypeStatusDto>
        {
            Success = true,
            Message = "Created",
            Data = dto
        };
    }
}
#endregion

#region Get / List
public record GetActivityTypeStatusByIdQuery(int ActivityTypeStatusId) : IRequest<ResponseResultDTO<ActivityTypeStatusDto?>>;
public record GetActivityTypeStatusListByTypeQuery(int ActivityTypeId, int CompanyId) : IRequest<ResponseResultDTO<List<ActivityTypeStatusDto>>>;

public class GetActivityTypeStatusHandler :
    IRequestHandler<GetActivityTypeStatusByIdQuery, ResponseResultDTO<ActivityTypeStatusDto?>>,
    IRequestHandler<GetActivityTypeStatusListByTypeQuery, ResponseResultDTO<List<ActivityTypeStatusDto>>>
{
    private readonly DBContextHRsystem _db;
    public GetActivityTypeStatusHandler(DBContextHRsystem db) => _db = db;

    public async Task<ResponseResultDTO<ActivityTypeStatusDto?>> Handle(GetActivityTypeStatusByIdQuery request, CancellationToken cancellationToken)
    {
        var e = await _db.TbActivityTypeStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ActivityTypeStatusId == request.ActivityTypeStatusId, cancellationToken);

        if (e == null)
            return new ResponseResultDTO<ActivityTypeStatusDto?> { Success = false, Message = "Not found" };

        var dto = new ActivityTypeStatusDto(e.ActivityTypeStatusId, e.ActivityTypeId, e.StatusId, e.IsDefault, e.IsActive, e.CompanyId);
        return new ResponseResultDTO<ActivityTypeStatusDto?> { Success = true, Data = dto };
    }

    public async Task<ResponseResultDTO<List<ActivityTypeStatusDto>>> Handle(GetActivityTypeStatusListByTypeQuery request, CancellationToken cancellationToken)
    {
        var list = await _db.TbActivityTypeStatuses
            .AsNoTracking()
            .Where(x => x.ActivityTypeId == request.ActivityTypeId && x.CompanyId == request.CompanyId)
            .Select(x => new ActivityTypeStatusDto(x.ActivityTypeStatusId, x.ActivityTypeId, x.StatusId, x.IsDefault, x.IsActive, x.CompanyId))
            .ToListAsync(cancellationToken);

        return new ResponseResultDTO<List<ActivityTypeStatusDto>> { Success = true, Data = list };
    }
}
#endregion

#region Update
public record UpdateActivityTypeStatusCommand(
    int ActivityTypeStatusId,
    int ActivityTypeId,
    int StatusId,
    bool IsDefault,
    bool IsAllowed,
    int CompanyId
) : IRequest<ResponseResultDTO<ActivityTypeStatusDto>>;

public class UpdateActivityTypeStatusValidator : AbstractValidator<UpdateActivityTypeStatusCommand>
{
    public UpdateActivityTypeStatusValidator()
    {
        RuleFor(x => x.ActivityTypeStatusId).GreaterThan(0);
        RuleFor(x => x.ActivityTypeId).GreaterThan(0);
        RuleFor(x => x.StatusId).GreaterThan(0);
        RuleFor(x => x.CompanyId).GreaterThan(0);
    }
}

public class UpdateActivityTypeStatusHandler : IRequestHandler<UpdateActivityTypeStatusCommand, ResponseResultDTO<ActivityTypeStatusDto>>
{
    private readonly DBContextHRsystem _db;
    public UpdateActivityTypeStatusHandler(DBContextHRsystem db) => _db = db;

    public async Task<ResponseResultDTO<ActivityTypeStatusDto>> Handle(UpdateActivityTypeStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.TbActivityTypeStatuses.FirstOrDefaultAsync(x => x.ActivityTypeStatusId == request.ActivityTypeStatusId, cancellationToken);
        if (entity == null)
        {
            return new ResponseResultDTO<ActivityTypeStatusDto> { Success = false, Message = "Not found" };
        }

        var errors = new List<ResponseErrorDTO>();
        var existsActivityType = await _db.TbActivityTypes.AnyAsync(x => x.ActivityTypeId == request.ActivityTypeId && x.CompanyId == request.CompanyId, cancellationToken);
        if (!existsActivityType)
            errors.Add(new ResponseErrorDTO { Property = nameof(request.ActivityTypeId), Error = $"ActivityTypeId {request.ActivityTypeId} not found for company {request.CompanyId}." });

        var existsStatus = await _db.TbActivityStatuses.AnyAsync(x => x.StatusId == request.StatusId && (x.CompanyId == null || x.CompanyId == request.CompanyId), cancellationToken);
        if (!existsStatus)
            errors.Add(new ResponseErrorDTO { Property = nameof(request.StatusId), Error = $"StatusId {request.StatusId} not found or not available for company {request.CompanyId}." });

        if (errors.Any())
        {
            return new ResponseResultDTO<ActivityTypeStatusDto>
            {
                Success = false,
                Message = "Validation failed",
                Errors = errors
            };
        }

        // if setting default true, clear other defaults
        if (request.IsDefault)
        {
            var others = await _db.TbActivityTypeStatuses
                .Where(x => x.ActivityTypeId == request.ActivityTypeId && x.CompanyId == request.CompanyId && x.IsDefault && x.ActivityTypeStatusId != request.ActivityTypeStatusId)
                .ToListAsync(cancellationToken);
            foreach (var o in others) o.IsDefault = false;
        }

        entity.ActivityTypeId = request.ActivityTypeId;
        entity.StatusId = request.StatusId;
        entity.IsDefault = request.IsDefault;
        entity.IsActive = request.IsAllowed;
        entity.CompanyId = request.CompanyId;

        await _db.SaveChangesAsync(cancellationToken);

        var dto = new ActivityTypeStatusDto(entity.ActivityTypeStatusId, entity.ActivityTypeId, entity.StatusId, entity.IsDefault, entity.IsActive, entity.CompanyId);
        return new ResponseResultDTO<ActivityTypeStatusDto> { Success = true, Message = "Updated", Data = dto };
    }
}
#endregion


#region Delete
public record DeleteActivityTypeStatusCommand(int ActivityTypeStatusId) : IRequest<ResponseResultDTO>;

public class DeleteActivityTypeStatusHandler : IRequestHandler<DeleteActivityTypeStatusCommand, ResponseResultDTO>
{
    private readonly DBContextHRsystem _db;
    public DeleteActivityTypeStatusHandler(DBContextHRsystem db) => _db = db;

    public async Task<ResponseResultDTO> Handle(DeleteActivityTypeStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.TbActivityTypeStatuses.FirstOrDefaultAsync(x => x.ActivityTypeStatusId == request.ActivityTypeStatusId, cancellationToken);
        if (entity == null)
            return new ResponseResultDTO { Success = false, Message = "Not found" };

        _db.TbActivityTypeStatuses.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return new ResponseResultDTO { Success = true, Message = "Deleted" };
    }
}
    #endregion
}
