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
    public static class AspRolePermissionsEndpoints
    {
        public static void MapAspRolePermissionsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/systemadmin/ListRolePermissions", [Authorize] async (IMediator mediator, int? roleId) =>
                await mediator.Send(new ListRolePermissionsQuery(roleId)))

              ////  this is an   OR
              .RequireAuthorization(policy =>
               policy.RequireAssertion(ctx =>
                ctx.User.IsInRole("SystemAdmin") ||
                ctx.User.HasClaim("Permission", "CanViewPermissions")))

            ////  this is an AND  
            //   .RequireAuthorization(policy => policy
            //.RequireRole("FinanceAdmin")
            //.RequireClaim("permission", "CanManagePermissions"))

                .WithName("ListRolePermissions")
                .WithTags("User Role Permission Management");

            app.MapGet("/api/systemadmin/GetRolePermission/{roleId}/{permissionId}", [Authorize] async (IMediator mediator, int roleId, int permissionId) =>
                await mediator.Send(new GetRolePermissionQuery(roleId, permissionId)))
                .WithName("GetRolePermission")
                .WithTags("User Role Permission Management");

            app.MapPost("/api/systemadmin/CreateRolePermission", [Authorize] async (IMediator mediator, CreateRolePermissionCommand cmd) =>
                await mediator.Send(cmd))
                .WithName("CreateRolePermission")
                .WithTags("User Role Permission Management");

            app.MapDelete("/api/systemadmin/DeleteRolePermission/{roleId}/{permissionId}", [Authorize] async (IMediator mediator, int roleId, int permissionId) =>
                await mediator.Send(new DeleteRolePermissionCommand(roleId, permissionId)))
                .WithName("DeleteRolePermission")
                .WithTags("User Role Permission Management");

            app.MapPut("/api/systemadmin/UpdateRolePermissions", [Authorize] async (IMediator mediator, UpdateRolePermissionsDto dto) =>
                await mediator.Send(new UpdateRolePermissionsCommand(dto)))
                .WithName("UpdateRolePermissions")
                .WithTags("User Role Permission Management");
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
        public bool IsSelected { get; set; }
    }

    public class UpdateRolePermissionsDto
    {
        public int RoleId { get; set; }
        public List<RolePermissionDto> Permissions { get; set; } = new();
    }
    #endregion

    #region Queries & Commands
    public record ListRolePermissionsQuery(int? RoleId = null) : IRequest<ResponseResultDTO<List<RolePermissionDto>>>;

    public record GetRolePermissionQuery(int RoleId, int PermissionId) : IRequest<ResponseResultDTO<AspRolePermissions>>;

    public record CreateRolePermissionCommand(int RoleId, int PermissionId) : IRequest<ResponseResultDTO<AspRolePermissions>>;

    public record DeleteRolePermissionCommand(int RoleId, int PermissionId) : IRequest<ResponseResultDTO<bool>>;

    public record UpdateRolePermissionsCommand(UpdateRolePermissionsDto Request) : IRequest<ResponseResultDTO<bool>>;
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
    public class ListRolePermissionsHandler(DBContextHRsystem db) : IRequestHandler<ListRolePermissionsQuery, ResponseResultDTO<List<RolePermissionDto>>>
    {
        public async Task<ResponseResultDTO<List<RolePermissionDto>>> Handle(ListRolePermissionsQuery request, CancellationToken ct)
        {
            var permissions = await db.AspPermissions
                .AsNoTracking()
                .Select(p => new { p.PermissionId, p.PermissionName, p.PermissionDescription })
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
                IsSelected = rolePermissionIds.Contains(p.PermissionId)
            }).ToList();

            return new ResponseResultDTO<List<RolePermissionDto>>
            {
                Success = true,
                Message = "Permissions list returned successfully",
                Data = result
            };
        }
    }

    public class GetRolePermissionHandler(DBContextHRsystem db) : IRequestHandler<GetRolePermissionQuery, ResponseResultDTO<AspRolePermissions>>
    {
        public async Task<ResponseResultDTO<AspRolePermissions>> Handle(GetRolePermissionQuery request, CancellationToken ct)
        {
            var entity = await db.Set<AspRolePermissions>()
                .FindAsync(new object[] { request.RoleId, request.PermissionId }, ct);

            return new ResponseResultDTO<AspRolePermissions>
            {
                Success = entity != null,
                Message = entity != null ? "Record found" : "Not found",
                Data = entity
            };
        }
    }

    public class CreateRolePermissionHandler(DBContextHRsystem db, ICurrentUserService userService) : IRequestHandler<CreateRolePermissionCommand, ResponseResultDTO<AspRolePermissions>>
    {
        public async Task<ResponseResultDTO<AspRolePermissions>> Handle(CreateRolePermissionCommand request, CancellationToken ct)
        {
            var exists = await db.Set<AspRolePermissions>()
                .AnyAsync(x => x.RoleId == request.RoleId && x.PermissionId == request.PermissionId, ct);

            if (exists)
                return new ResponseResultDTO<AspRolePermissions> { Success = false, Message = "RolePermission already exists" };

            var entity = new AspRolePermissions
            {
                RoleId = request.RoleId,
                PermissionId = request.PermissionId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userService.UserId
            };

            db.Set<AspRolePermissions>().Add(entity);
            await db.SaveChangesAsync(ct);

            return new ResponseResultDTO<AspRolePermissions>
            {
                Success = true,
                Message = "RolePermission created successfully",
                Data = entity
            };
        }
    }

    public class DeleteRolePermissionHandler(DBContextHRsystem db) : IRequestHandler<DeleteRolePermissionCommand, ResponseResultDTO<bool>>
    {
        public async Task<ResponseResultDTO<bool>> Handle(DeleteRolePermissionCommand request, CancellationToken ct)
        {
            var entity = await db.Set<AspRolePermissions>()
                .FindAsync(new object[] { request.RoleId, request.PermissionId }, ct);

            if (entity == null)
                return new ResponseResultDTO<bool> { Success = false, Message = "Not found", Data = false };

            db.Set<AspRolePermissions>().Remove(entity);
            await db.SaveChangesAsync(ct);

            return new ResponseResultDTO<bool>
            {
                Success = true,
                Message = "Deleted successfully",
                Data = true
            };
        }
    }

    public class UpdateRolePermissionsHandler(DBContextHRsystem db, ICurrentUserService userService) : IRequestHandler<UpdateRolePermissionsCommand, ResponseResultDTO<bool>>
    {
        public async Task<ResponseResultDTO<bool>> Handle(UpdateRolePermissionsCommand request, CancellationToken ct)
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

            return new ResponseResultDTO<bool>
            {
                Success = true,
                Message = "Role permissions updated successfully",
                Data = true
            };
        }
    }
    #endregion
}
