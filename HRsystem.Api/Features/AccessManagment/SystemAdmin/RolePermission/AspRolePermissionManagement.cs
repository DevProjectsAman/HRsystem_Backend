using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.SystemAdmin.RolePermission
{

    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.Mvc; // For [Authorize]
    using System.Threading.Tasks;

    // --- Assumed DTO Definitions (as before) ---
    // public record ResponseErrorDTO(string Property, string Error);
    // public class ResponseResultDTO { /* ... */ }
    // public class ResponseResultDTO<T> : ResponseResultDTO { /* ... */ }
    // public class UpdateRolePermissionsDto { /* ... */ } 
    // -------------------------------------------

    public static class AspRolePermissionsEndpoints
    {
        public static void MapAspRolePermissionsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/AccessManagement").WithTags("_UserManagement");

            // Assuming ListRolePermissionsQuery returns IEnumerable<RolePermissionDto> or a list.
            group.MapGet("/ListRolePermissions", [Authorize] async (IMediator mediator, int? roleId) =>
            {
                var result = await mediator.Send(new ListRolePermissionsQuery(roleId));

                return Results.Ok(new ResponseResultDTO<object>
                {
                    Success = true,
                    Message = $"Role permissions retrieved successfully for Role ID: {roleId ?? 0}",
                    Data = result
                });
            })
            // Apply the custom authorization policy
            .RequireAuthorization(policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.IsInRole("SystemAdmin") &&
                    ctx.User.HasClaim("permission", "system.role.permissions")))
            .WithName("ListRolePermissions");

            // --- Get Role Permission ---
            // Assuming GetRolePermissionQuery returns RolePermissionDto or null.
            group.MapGet("/GetRolePermission/{roleId}/{permissionId}", [Authorize] async (IMediator mediator, int roleId, int permissionId) =>
            {
                var result = await mediator.Send(new GetRolePermissionQuery(roleId, permissionId));

                if (result == null)
                {
                    return Results.NotFound(new ResponseResultDTO
                    {
                        Success = false,
                        Message = $"RolePermission for Role ID {roleId} and Permission ID {permissionId} not found"
                    });
                }

                return Results.Ok(new ResponseResultDTO<object>
                {
                    Success = true,
                    Message = "Role permission retrieved successfully",
                    Data = result
                });
            })
                   .RequireAuthorization(policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.IsInRole("SystemAdmin") &&
                     ctx.User.HasClaim("permission", "system.role.permissions")))
            .WithName("GetRolePermission");

            // --- Create Role Permission ---
            // Assuming CreateRolePermissionCommand returns the newly created DTO/ID.
            group.MapPost("/CreateRolePermission", [Authorize] async (IMediator mediator, CreateRolePermissionCommand cmd) =>
            {
                var result = await mediator.Send(cmd);

                // Assuming 'result' contains the data of the newly created link
                // For simplicity, we use a generic success message.
                return Results.Created(
                    "/api/AccessManagement/CreateRolePermission", // Can be more specific if result contains IDs
                    new ResponseResultDTO<object>
                    {
                        Success = true,
                        Message = "Role permission created successfully",
                        Data = result
                    });
            })
                   .RequireAuthorization(policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.IsInRole("SystemAdmin") &&
                     ctx.User.HasClaim("permission", "system.role.permissions")))
            .WithName("CreateRolePermission");

            // --- Delete Role Permission ---
            // Assuming DeleteRolePermissionCommand returns a bool (true for deleted, false for not found).
            // NOTE: Adjusted route path to align with the group prefix (/api/AccessManagement) if possible, 
            // but kept original for now since it's an absolute path override.
            group.MapDelete("/DeleteRolePermission/{roleId}/{permissionId}", [Authorize] async (IMediator mediator, int roleId, int permissionId) =>
            {
                var result = await mediator.Send(new DeleteRolePermissionCommand(roleId, permissionId));



                return Results.Ok(new ResponseResultDTO
                {
                    Success = true,
                    Message = $"RolePermission deleted successfully"
                });
            })
                   .RequireAuthorization(policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.IsInRole("SystemAdmin") &&
                     ctx.User.HasClaim("permission", "system.role.permissions")))
            .WithName("DeleteRolePermission");

            // --- Update Role Permissions (Bulk) ---
            // Assuming UpdateRolePermissionsCommand returns a summary object or true/false.
            // Assuming the route path is intended to be absolute, but removing the group prefix for now.
            group.MapPut("/UpdateRolePermissions", [Authorize] async (IMediator mediator, UpdateRolePermissionsDto dto) =>
            {
                var result = await mediator.Send(new UpdateRolePermissionsCommand(dto));



                return Results.Ok(new ResponseResultDTO<object>
                {
                    Success = true,
                    Message = "Role permissions updated successfully",
                    Data = result
                });
            })
                   .RequireAuthorization(policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.IsInRole("SystemAdmin") &&
                     ctx.User.HasClaim("permission", "system.role.permissions")))
            .WithName("UpdateRolePermissions");
        }
    }

    #region Entities & DTOs
    //public class AspRolePermission
    //{
    //    public int RoleId { get; set; }
    //    public int PermissionId { get; set; }
    //    public DateTime? CreatedAt { get; set; }
    //    public int? CreatedBy { get; set; }
    //}



    public class RolePermissionDto
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public string PermissionDescription { get; set; } = string.Empty;

        public string PermissionCatagory { get; set; }

        public bool IsSelected { get; set; }
    }

    public class UpdateRolePermissionsDto
    {
        public int RoleId { get; set; }
        public List<RolePermissionDto> Permissions { get; set; } = new();
    }
    #endregion

    #region Queries & Commands
    public record ListRolePermissionsQuery(int? RoleId = null) : IRequest<List<RolePermissionDto>>;

    public record GetRolePermissionQuery(int RoleId, int PermissionId) : IRequest<AspRolePermissions>;

    public record CreateRolePermissionCommand(int RoleId, int PermissionId) : IRequest<bool>;

    public record DeleteRolePermissionCommand(int RoleId, int PermissionId) : IRequest<bool>;

    public record UpdateRolePermissionsCommand(UpdateRolePermissionsDto Request) : IRequest<bool>;
    #endregion

    #region Validators
    public class CreateRolePermissionValidator : AbstractValidator<CreateRolePermissionCommand>
    {
        public CreateRolePermissionValidator()
        {
            RuleFor(x => x.RoleId).GreaterThan(0);
            RuleFor(x => x.PermissionId).GreaterThan(0);
        }
    }

    public class UpdateRolePermissionsValidator : AbstractValidator<UpdateRolePermissionsCommand>
    {
        public UpdateRolePermissionsValidator()
        {
            RuleFor(x => x.Request.RoleId).GreaterThan(0);

            RuleFor(x => x.Request.Permissions)
                .NotEmpty().WithMessage("At least one permission must be provided");

            RuleFor(x => x.Request.Permissions.Count(p => p.IsSelected))
                .GreaterThan(0).WithMessage("At least one permission must be selected");

            RuleForEach(x => x.Request.Permissions)
                .ChildRules(permission =>
                {
                    permission.RuleFor(p => p.PermissionId).GreaterThan(0);
                    permission.RuleFor(p => p.PermissionName).NotEmpty();
                });
        }
    }
    #endregion

    #region Handlers
    public class ListRolePermissionsHandler(DBContextHRsystem db) : IRequestHandler<ListRolePermissionsQuery, List<RolePermissionDto>>
    {
        public async Task<List<RolePermissionDto>> Handle(ListRolePermissionsQuery request, CancellationToken ct)
        {
            var permissions = await db.AspPermissions
                .AsNoTracking()
                .Select(p => new { p.PermissionId, p.PermissionName, p.PermissionDescription, p.PermissionCatagory })
                .ToListAsync(ct);

            var rolePermissionIds = new HashSet<int>();
            if (request.RoleId.HasValue)
            {
                rolePermissionIds = (await db.AspRolePermissions
                    .Where(rp => rp.RoleId == request.RoleId.Value)
                    .Select(rp => rp.PermissionId)
                    .ToListAsync(ct))
                    .ToHashSet();
            }

            var result = permissions.Select(p => new RolePermissionDto
            {
                PermissionId = p.PermissionId,
                PermissionName = p.PermissionName,
                PermissionDescription = p.PermissionDescription,
                PermissionCatagory = p.PermissionCatagory,

                IsSelected = rolePermissionIds.Contains(p.PermissionId)
            }).ToList();

            return result;
        }
    }

    public class GetRolePermissionHandler(DBContextHRsystem db) : IRequestHandler<GetRolePermissionQuery, AspRolePermissions>
    {
        public async Task<AspRolePermissions> Handle(GetRolePermissionQuery request, CancellationToken ct)
        {
            var entity = await db.Set<AspRolePermissions>()
                .FindAsync(new object[] { request.RoleId, request.PermissionId }, ct);

            return entity;
        }
    }

    public class CreateRolePermissionHandler(DBContextHRsystem db, ICurrentUserService userService) : IRequestHandler<CreateRolePermissionCommand, bool>
    {
        public async Task<bool> Handle(CreateRolePermissionCommand request, CancellationToken ct)
        {
            var exists = await db.Set<AspRolePermissions>()
                .AnyAsync(x => x.RoleId == request.RoleId && x.PermissionId == request.PermissionId, ct);

            if (exists)
                return false;

            var entity = new AspRolePermissions
            {
                RoleId = request.RoleId,
                PermissionId = request.PermissionId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userService.UserId
            };

            db.Set<AspRolePermissions>().Add(entity);
            await db.SaveChangesAsync(ct);

            return true;
        }
    }

    public class DeleteRolePermissionHandler(DBContextHRsystem db) : IRequestHandler<DeleteRolePermissionCommand, bool>
    {
        public async Task<bool> Handle(DeleteRolePermissionCommand request, CancellationToken ct)
        {
            var entity = await db.Set<AspRolePermissions>()
                .FindAsync(new object[] { request.RoleId, request.PermissionId }, ct);

            if (entity == null)
                return false;

            db.Set<AspRolePermissions>().Remove(entity);
            await db.SaveChangesAsync(ct);

            return true;
        }
    }

    public class UpdateRolePermissionsHandler(DBContextHRsystem db, ICurrentUserService userService) : IRequestHandler<UpdateRolePermissionsCommand, bool>
    {
        public async Task<bool> Handle(UpdateRolePermissionsCommand request, CancellationToken ct)
        {
            var roleId = request.Request.RoleId;
            var selectedPermissions = request.Request.Permissions
                .Where(p => p.IsSelected)
                .Select(p => p.PermissionId)
                .ToList();

            using var trx = await db.Database.BeginTransactionAsync(ct);

            // Remove old permissions
            var oldPermissions = db.AspRolePermissions.Where(rp => rp.RoleId == roleId);
            db.AspRolePermissions.RemoveRange(oldPermissions);

            // Add new permissions
            foreach (var pid in selectedPermissions)
            {
                db.AspRolePermissions.Add(new AspRolePermissions
                {
                    RoleId = roleId,
                    PermissionId = pid,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userService.UserId
                });
            }

            await db.SaveChangesAsync(ct);
            await trx.CommitAsync(ct);

            return true;
        }
    }
    #endregion
}
