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
    public static class AspPermissionsEndpoints
    {
        public static void MapAspPermissionsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/systemadmin/ListPermissions", [Authorize] async (IMediator mediator) =>
                await mediator.Send(new GetAllPermissionsQuery()))
                .WithName("ListPermissions")
                .WithTags("User Role Permission Management");

            app.MapGet("/api/systemadmin/GetOnePermission/{id}", [Authorize] async (IMediator mediator, int id) =>
                await mediator.Send(new GetPermissionByIdQuery(id)))
                .WithName("GetOnePermission")
                .WithTags("User Role Permission Management");

            app.MapPost("/api/systemadmin/CreatePermission", [Authorize] async (IMediator mediator, CreatePermissionCommand cmd) =>
                await mediator.Send(cmd))
                .WithName("CreatePermission")
                .WithTags("User Role Permission Management");

            app.MapPut("/api/systemadmin/UpdatePermission", [Authorize] async (IMediator mediator, UpdatePermissionCommand cmd) =>
             await mediator.Send(cmd))
            .WithName("UpdatePermission")
            .WithTags("User Role Permission Management");

            app.MapDelete("/api/systemadmin/DeletePermission/{id}", [Authorize] async (IMediator mediator, int id) =>
                await mediator.Send(new DeletePermissionCommand(id)))
                .WithName("DeletePermission")
                .WithTags("User Role Permission Management");
        }
    }


    #region Get All
    public record GetAllPermissionsQuery : IRequest<ResponseResultDTO<List<AspPermission>>>;

    public class GetAllPermissionsHandler(DBContextHRsystem db) : IRequestHandler<GetAllPermissionsQuery, ResponseResultDTO<List<AspPermission>>>
    {
        public async Task<ResponseResultDTO<List<AspPermission>>> Handle(GetAllPermissionsQuery request, CancellationToken ct)
        {
            var data = await db.AspPermissions.AsNoTracking().ToListAsync(ct);

            return new ResponseResultDTO<List<AspPermission>>
            {
                Success = true,
                Message = "Permissions returned successfully",
                Data = data
            };
        }
    }
    #endregion

    #region Get One
    public record GetPermissionByIdQuery(int Id) : IRequest<ResponseResultDTO<AspPermission?>>;

    public class GetPermissionByIdHandler(DBContextHRsystem db) : IRequestHandler<GetPermissionByIdQuery, ResponseResultDTO<AspPermission?>>
    {
        public async Task<ResponseResultDTO<AspPermission?>> Handle(GetPermissionByIdQuery request, CancellationToken ct)
        {
            var entity = await db.AspPermissions.AsNoTracking()
                .FirstOrDefaultAsync(x => x.PermissionId == request.Id, ct);

            return new ResponseResultDTO<AspPermission?>
            {
                Success = entity != null,
                Message = entity != null ? "Permission found" : "Not found",
                Data = entity
            };
        }
    }
    #endregion

    #region Create
    public record CreatePermissionCommand(
        string PermissionCatagory,
        string PermissionName,
        string PermissionDescription
    ) : IRequest<ResponseResultDTO<int>>;

    public class CreatePermissionValidator : AbstractValidator<CreatePermissionCommand>
    {
        public CreatePermissionValidator()
        {
            RuleFor(x => x.PermissionCatagory).NotEmpty().MaximumLength(50);
            RuleFor(x => x.PermissionName).NotEmpty().MaximumLength(80);
            RuleFor(x => x.PermissionDescription).NotEmpty().MaximumLength(100);
        }
    }

    public class CreatePermissionHandler(DBContextHRsystem db, ICurrentUserService userService)
        : IRequestHandler<CreatePermissionCommand, ResponseResultDTO<int>>
    {
        public async Task<ResponseResultDTO<int>> Handle(CreatePermissionCommand request, CancellationToken ct)
        {
            var exists = await db.AspPermissions.AnyAsync(x => x.PermissionName == request.PermissionName, ct);
            if (exists)
            {
                return new ResponseResultDTO<int>
                {
                    Success = false,
                    Message = "Permission name already exists"
                };
            }

            var entity = new AspPermission
            {
                PermissionCatagory = request.PermissionCatagory,
                PermissionName = request.PermissionName,
                PermissionDescription = request.PermissionDescription,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userService.UserId
            };

            db.AspPermissions.Add(entity);
            await db.SaveChangesAsync(ct);

            return new ResponseResultDTO<int>
            {
                Success = true,
                Message = "Permission created successfully",
                Data = entity.PermissionId
            };
        }
    }
    #endregion

    #region Update
    public record UpdatePermissionCommand : IRequest<ResponseResultDTO<bool>>
    {
        public int PermissionId { get; set; }
        public string PermissionCatagory { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;
        public string PermissionDescription { get; set; } = string.Empty;
    }

    public class UpdatePermissionValidator : AbstractValidator<UpdatePermissionCommand>
    {
        public UpdatePermissionValidator()
        {
            RuleFor(x => x.PermissionId).GreaterThan(0);
            RuleFor(x => x.PermissionCatagory).NotEmpty().MaximumLength(50);
            RuleFor(x => x.PermissionName).NotEmpty().MaximumLength(80);
            RuleFor(x => x.PermissionDescription).NotEmpty().MaximumLength(100);
        }
    }

    public class UpdatePermissionHandler(DBContextHRsystem db, ICurrentUserService userService)
        : IRequestHandler<UpdatePermissionCommand, ResponseResultDTO<bool>>
    {
        public async Task<ResponseResultDTO<bool>> Handle(UpdatePermissionCommand request, CancellationToken ct)
        {
            var entity = await db.AspPermissions.FindAsync(new object?[] { request.PermissionId }, ct);
            if (entity == null)
                return new ResponseResultDTO<bool> { Success = false, Message = "Not found" };


             

            // check duplicate name
            var exists = await db.AspPermissions
                .AnyAsync(x => x.PermissionName == request.PermissionName && x.PermissionId != request.PermissionId, ct);

            if (exists)
                return new ResponseResultDTO<bool> { Success = false, Message = "Permission name already exists" };

            entity.PermissionCatagory = request.PermissionCatagory;
            entity.PermissionName = request.PermissionName;
            entity.PermissionDescription = request.PermissionDescription;
            entity.CreatedAt = entity.CreatedAt; // keep original
            entity.CreatedBy = entity.CreatedBy; // keep original

            await db.SaveChangesAsync(ct);

            return new ResponseResultDTO<bool> { Success = true, Message = "Updated successfully", Data = true };
        }
    }
    #endregion

    #region Delete
    public record DeletePermissionCommand(int Id) : IRequest<ResponseResultDTO<bool>>;

    public class DeletePermissionHandler(DBContextHRsystem db)
        : IRequestHandler<DeletePermissionCommand, ResponseResultDTO<bool>>
    {
        public async Task<ResponseResultDTO<bool>> Handle(DeletePermissionCommand request, CancellationToken ct)
        {
            var entity = await db.AspPermissions.FindAsync(new object?[] { request.Id }, ct);
            if (entity == null)
                return new ResponseResultDTO<bool> { Success = false, Message = "Not found" };

            db.AspPermissions.Remove(entity);
            await db.SaveChangesAsync(ct);

            return new ResponseResultDTO<bool> { Success = true, Message = "Deleted successfully", Data = true };
        }
    }
    #endregion





}
