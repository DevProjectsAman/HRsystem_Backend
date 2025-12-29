using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Lookups.ContractTypes;

// --------------------------------------------------
// DTO
// --------------------------------------------------
public class ContractTypeDto
{
    public int ContractTypeId { get; set; }
    public string? ContractTypeCode { get; set; }
    public string ContractTypeName { get; set; }
}

// --------------------------------------------------
// CREATE
// --------------------------------------------------
public record CreateContractTypeCommand(string? ContractTypeCode, string ContractTypeName)
    : IRequest<ContractTypeDto>;

public class CreateContractTypeHandler
    : IRequestHandler<CreateContractTypeCommand, ContractTypeDto>
{
    private readonly DBContextHRsystem _db;

    public CreateContractTypeHandler(DBContextHRsystem db) => _db = db;

    public async Task<ContractTypeDto> Handle(CreateContractTypeCommand request, CancellationToken ct)
    {
        var entity = new TbContractType
        {
            ContractTypeCode = request.ContractTypeCode,
            ContractTypeName = request.ContractTypeName
        };

        _db.TbContractTypes.Add(entity);
        await _db.SaveChangesAsync(ct);

        return new ContractTypeDto
        {
            ContractTypeId = entity.ContractTypeId,
            ContractTypeCode = entity.ContractTypeCode,
            ContractTypeName = entity.ContractTypeName
        };
    }
}

// --------------------------------------------------
// UPDATE
// --------------------------------------------------
public record UpdateContractTypeCommand(int ContractTypeId, string? ContractTypeCode, string ContractTypeName)
    : IRequest<bool>;

public class UpdateContractTypeHandler
    : IRequestHandler<UpdateContractTypeCommand, bool>
{
    private readonly DBContextHRsystem _db;

    public UpdateContractTypeHandler(DBContextHRsystem db) => _db = db;

    public async Task<bool> Handle(UpdateContractTypeCommand request, CancellationToken ct)
    {
        var entity = await _db.TbContractTypes
            .FirstOrDefaultAsync(x => x.ContractTypeId == request.ContractTypeId, ct);

        if (entity == null)
            return false;

        entity.ContractTypeCode = request.ContractTypeCode;
        entity.ContractTypeName = request.ContractTypeName;

        await _db.SaveChangesAsync(ct);
        return true;
    }
}

// --------------------------------------------------
// DELETE
// --------------------------------------------------
public record DeleteContractTypeCommand(int ContractTypeId) : IRequest<bool>;

public class DeleteContractTypeHandler
    : IRequestHandler<DeleteContractTypeCommand, bool>
{
    private readonly DBContextHRsystem _db;

    public DeleteContractTypeHandler(DBContextHRsystem db) => _db = db;

    public async Task<bool> Handle(DeleteContractTypeCommand request, CancellationToken ct)
    {
        var entity = await _db.TbContractTypes
            .FirstOrDefaultAsync(x => x.ContractTypeId == request.ContractTypeId, ct);

        if (entity == null)
            return false;

        _db.TbContractTypes.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}

// --------------------------------------------------
// GET ALL
// --------------------------------------------------
public record GetAllContractTypesQuery() : IRequest<List<ContractTypeDto>>;

public class GetAllContractTypesHandler
    : IRequestHandler<GetAllContractTypesQuery, List<ContractTypeDto>>
{
    private readonly DBContextHRsystem _db;

    public GetAllContractTypesHandler(DBContextHRsystem db) => _db = db;

    public async Task<List<ContractTypeDto>> Handle(GetAllContractTypesQuery request, CancellationToken ct)
    {
        return await _db.TbContractTypes
            .Select(x => new ContractTypeDto
            {
                ContractTypeId = x.ContractTypeId,
                ContractTypeCode = x.ContractTypeCode,
                ContractTypeName = x.ContractTypeName
            })
            .ToListAsync(ct);
    }
}

// --------------------------------------------------
// GET BY ID
// --------------------------------------------------
public record GetContractTypeByIdQuery(int ContractTypeId) : IRequest<ContractTypeDto?>;

public class GetContractTypeByIdHandler
    : IRequestHandler<GetContractTypeByIdQuery, ContractTypeDto?>
{
    private readonly DBContextHRsystem _db;

    public GetContractTypeByIdHandler(DBContextHRsystem db) => _db = db;

    public async Task<ContractTypeDto?> Handle(GetContractTypeByIdQuery request, CancellationToken ct)
    {
        return await _db.TbContractTypes
            .Where(x => x.ContractTypeId == request.ContractTypeId)
            .Select(x => new ContractTypeDto
            {
                ContractTypeId = x.ContractTypeId,
                ContractTypeCode = x.ContractTypeCode,
                ContractTypeName = x.ContractTypeName
            })
            .FirstOrDefaultAsync(ct);
    }
}

// --------------------------------------------------
// ENDPOINTS (Minimal API)
// --------------------------------------------------
public static class ContractTypeEndpoints
{
    public static IEndpointRouteBuilder MapContractTypeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Lookups/contracttypes");

        group.MapGet("/GetAllContractTypes", [Authorize] async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllContractTypesQuery());
            return Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
        });

        group.MapGet("/GetContractTypeById{id:int}", [Authorize] async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetContractTypeByIdQuery(id));
            return Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
        });

        group.MapPost("/CreateNewContractType", [Authorize] async (CreateContractTypeCommand cmd, IMediator mediator) =>
        {
            var result = await mediator.Send(cmd);
            return Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
        });


            

        group.MapPut("/UpdateContractType{id:int}", [Authorize] async (int id, UpdateContractTypeCommand cmd, IMediator mediator) =>
        {
            if (id != cmd.ContractTypeId) return Results.BadRequest(new ResponseResultDTO() { Success = false, Message = "IDs do not match" });
            var success = await mediator.Send(cmd);
           // return success ? Results.Ok() : Results.NotFound();
           if(!success)
                return Results.NotFound(new ResponseResultDTO() { Success = false, Message = "Contract Type not found" });
           else
            return Results.Ok(new ResponseResultDTO<object> { Success = true, Data = true });
        });

        group.MapDelete("/DeleteContractType{id:int}", [Authorize] async (int id, IMediator mediator) =>
        {
            var success = await mediator.Send(new DeleteContractTypeCommand(id));
            if (!success)
                return Results.NotFound(new ResponseResultDTO() { Success = false, Message = "Contract Type not found" });
            else
                return Results.Ok(new ResponseResultDTO<object> { Success = true, Data = true });
        });

        return app;
    }


}
