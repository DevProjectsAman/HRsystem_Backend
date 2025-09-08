using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Features.SystemAdmin.DTO;
using HRsystem.Api.Services.Auth;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SequentialGuid;

namespace HRsystem.Api.Features.SystemAdmin.Permissions;

public static class PermissionEndpoints
{


    public static void MapPermissionEndpoints(this IEndpointRouteBuilder app)
    {

        app.MapGet("/api/get-permissions", async (IMediator mediator) =>
            await mediator.Send(new GetPermissionsQuery()))

             .RequireAuthorization("SystemAdmin:,SystemAdmin:CanViewPermissions") // 👈 correct for minimal API
             //  .RequireAuthorization("SystemAdmin:") // 👈 correct for minimal API
             //  .RequireAuthorization(":CanViewPermissions") // 👈 correct for minimal API

            .WithName("GetPermissions")
            .WithTags("Permission Management");

        app.MapPost("/api/add-permissions", async (IMediator mediator, AddPermissionCommand command) =>
            await mediator.Send(command))
           .RequireAuthorization(policy => policy
        .RequireRole("FinanceAdmin")
        .RequireClaim("permission", "CanManagePermissions"))

            .WithName("AddPermission")
            .WithTags("Permission Management");
    }



}

#region Get Permissions

public record GetPermissionsQuery : IRequest<ResponseResultDTO<List<PermissionDTO>>>;


public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, ResponseResultDTO<List<PermissionDTO>>>
{
    private readonly DBContextHRsystem _dbContext;

    public GetPermissionsHandler(DBContextHRsystem dbcontext)
    {
        _dbContext = dbcontext;
    }

    public Task<ResponseResultDTO<List<PermissionDTO>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var permissions = _dbContext.AspPermissions
                .Select(p => new PermissionDTO
                {
                    PermissionId = p.PermissionId,
                    PermissionCatagory = p.PermissionCatagory,
                    PermissionName = p.PermissionName,
                    PermissionDescription = p.PermissionDescription
                })
                .ToList();

            return Task.FromResult(new ResponseResultDTO<List<PermissionDTO>>
            {
                Success = true,
                Message = ResponseMessages.DataReturnedSucc,
                Data = permissions
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ResponseResultDTO<List<PermissionDTO>>
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            });
        }
    }
}

#endregion

#region Add Permission

public record AddPermissionCommand(PermissionDTO Permission) : IRequest<ResponseResultDTO>;



public class AddPermissionHandler : IRequestHandler<AddPermissionCommand, ResponseResultDTO>
{
    private readonly DBContextHRsystem _dbContext;

    private readonly ICurrentUserService _currentUser;

    public AddPermissionHandler(DBContextHRsystem dbContext, ICurrentUserService currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<ResponseResultDTO> Handle(AddPermissionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = new AspPermission
            {
               // PermissionId = SequentialGuidGenerator.Instance.NewGuid(),
                PermissionCatagory = request.Permission.PermissionCatagory,
                PermissionName = request.Permission.PermissionName,
                PermissionDescription = request.Permission.PermissionDescription,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId,
            };

            await _dbContext.AspPermissions.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return new ResponseResultDTO()
            {
                Success = true,
                Message = "Saved Successfully"
            };
        }
        catch (Exception ex)
        {
            return new ResponseResultDTO
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }
}

#endregion



