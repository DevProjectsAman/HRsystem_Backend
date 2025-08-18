using HRsystem.Api.Database.Entities;
using HRsystem.Api.Features.SystemAdmin.DTO;
using HRsystem.Api.Shared;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations;

namespace HRsystem.Api.Features.SystemAdmin.Roles;

public static class RoleManagementEndpoints
{
    public static void MapRoleManagement(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/get-all-roles", [Authorize] async (IMediator mediator) =>
            await mediator.Send(new GetRolesQuery()))
            .WithName("GetAllRoles")
            .WithTags("Role Management");

        app.MapPost("/api/add-new-roles", async (IMediator mediator, AddRoleCommand command) =>
            await mediator.Send(command))
            .WithName("AddRole")
            .WithTags("Role Management");

        app.MapGet("/api/roles/check-role-exists/{roleName}", async (IMediator mediator, string roleName) =>
            await mediator.Send(new RoleExistsQuery(roleName)))
            .WithName("RoleExists")
            .WithTags("Role Management");
    }

}


#region Get All Roles

public record GetRolesQuery : IRequest<ResponseResultDTO<List<RoleDTO>>>;

public class GetRolesHandler : IRequestHandler<GetRolesQuery, ResponseResultDTO<List<RoleDTO>>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public GetRolesHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public Task<ResponseResultDTO<List<RoleDTO>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var roles = _roleManager.Roles
                .Select(role => new RoleDTO
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    
                      
                })
                .ToList();


            // Prepare the ResultDTO object
            var result = new ResponseResultDTO<List<RoleDTO>>
            {
                Success = true,
                Message = ResponseMessages.DataReturnedSucc,
                Data = roles
            };

            return Task.FromResult(result);
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ResponseResultDTO<List<RoleDTO>>
            {
                Success = false,
                Message = $"Error occurred in GetRoleList: {ex.Message}"
            });
        }
    }
}

#endregion

#region Add Role

public record AddRoleCommand([Required] string RoleName) : IRequest<IdentityResult>;

public class AddRoleHandler : IRequestHandler<AddRoleCommand, IdentityResult>
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AddRoleHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var exists = await _roleManager.FindByNameAsync(request.RoleName);
        if (exists != null)
            throw new Exception("Role already exists.");

        var role = new ApplicationRole { Name = request.RoleName };
        return await _roleManager.CreateAsync(role);
    }
}

#endregion

#region Check Role Exists

public record RoleExistsQuery(string RoleName) : IRequest<bool>;

public class RoleExistsHandler : IRequestHandler<RoleExistsQuery, bool>
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RoleExistsHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> Handle(RoleExistsQuery request, CancellationToken cancellationToken)
    {
        return await _roleManager.FindByNameAsync(request.RoleName) is not null;
    }
}

#endregion
