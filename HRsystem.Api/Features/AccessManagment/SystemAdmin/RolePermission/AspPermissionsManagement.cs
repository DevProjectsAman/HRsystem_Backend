using FluentValidation;
using FluentValidation.Results; // Required for ValidationResult
using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// ===================================================================
// NOTE: Placeholder DTOs and Context/Entity Classes
// (Assuming these are correctly defined elsewhere in your project)
// ===================================================================

// Assuming your generic DTOs are defined like this:
/*
namespace HRsystem.Api.Shared.DTO
{
    public class ResponseErrorDTO
    {
        public string Property { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }

    public class ResponseResultDTO
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public List<ResponseErrorDTO>? Errors { get; set; }
    }

    public class ResponseResultDTO<T> : ResponseResultDTO
    {
        public T? Data { get; set; }
    }
}
*/

namespace HRsystem.Api.Features.AccessManagment.SystemAdmin.RolePermission
{
    public static class AspPermissionsEndpoints
    {
        // Helper to extract common logic for HTTP result returns
        private static IResult HandleCommandResult(ResponseResultDTO result, int? successStatusCode = 200)
        {
            if (result.Success)
            {
                return successStatusCode == 201
                    ? Results.Created(string.Empty, result)
                    : Results.Ok(result);
            }
            else if (result.Message.Contains("not found"))
            {
                return Results.NotFound(result); // 404
            }
            else
            {
                return Results.BadRequest(result); // 400 Bad Request
            }
        }

        // Helper to handle Validation check
        private static IResult HandleValidationFailure(ValidationResult validationResult)
        {
            return Results.BadRequest(new ResponseResultDTO
            {
                Success = false,
                Message = "Validation failed",
                // Correct use of object initializer for your ResponseErrorDTO class
                Errors = validationResult.Errors.Select(e => new ResponseErrorDTO
                {
                    Property = e.PropertyName,
                    Error = e.ErrorMessage
                }).ToList()
            });
        }


        public static void MapAspPermissionsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/AccessManagement")
                .WithTags("_UserManagement");

            // --- List Permissions (GET) ---
            group.MapGet("/ListPermissions", [Authorize] async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllPermissionsQuery());
                return result.Success
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            })
                   .RequireAuthorization(policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.IsInRole("SystemAdmin") &&
                    ctx.User.HasClaim("permission", "system.permissions")))
            .WithName("ListPermissions");

            // --- Get Permission by Id (GET) ---
            group.MapGet("/GetOnePermission/{id}", [Authorize] async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetPermissionByIdQuery(id));
                return result.Success
                    ? Results.Ok(result)
                    : Results.NotFound(result);
            })
                   .RequireAuthorization(policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.IsInRole("SystemAdmin") &&
                    ctx.User.HasClaim("permission", "system.permissions")))
            .WithName("GetOnePermission");

            // --- Create Permission (POST) ---
            group.MapPost("/CreatePermission", [Authorize] async (IMediator mediator, IValidator<CreatePermissionCommand> validator, CreatePermissionCommand cmd) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                {
                    return HandleValidationFailure(validationResult);
                }

                var result = await mediator.Send(cmd);
                return HandleCommandResult(result, successStatusCode: 201); // 201 Created
            })
                   .RequireAuthorization(policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.IsInRole("SystemAdmin") &&
                    ctx.User.HasClaim("permission", "system.permissions")))
            .WithName("CreatePermission");

            // --- Update Permission (PUT) ---
            group.MapPut("/UpdatePermission", [Authorize] async (IMediator mediator, IValidator<UpdatePermissionCommand> validator, UpdatePermissionCommand cmd) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                {
                    return HandleValidationFailure(validationResult);
                }

                var result = await mediator.Send(cmd);
                return HandleCommandResult(result); // 200 OK
            })
                   .RequireAuthorization(policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.IsInRole("SystemAdmin") &&
                    ctx.User.HasClaim("permission", "system.permissions")))
            .WithName("UpdatePermission");

            // --- Delete Permission (DELETE) ---
            group.MapDelete("/DeletePermission/{id}", [Authorize] async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new DeletePermissionCommand(id));
                return HandleCommandResult(result); // Handles 404 (not found) and 200 (success)
            })
                   .RequireAuthorization(policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.IsInRole("SystemAdmin") &&
                    ctx.User.HasClaim("permission", "system.permissions")))
            .WithName("DeletePermission");
        }
    }


    #region Requests (Queries & Commands) - Updated Return Types
    public record GetAllPermissionsQuery : IRequest<ResponseResultDTO<List<AspPermission>>>;

    public record GetPermissionByIdQuery(int Id) : IRequest<ResponseResultDTO<AspPermission>>;

    public record CreatePermissionCommand(
        string PermissionCatagory,
        string PermissionName,
        string PermissionDescription
    ) : IRequest<ResponseResultDTO>;

    public record UpdatePermissionCommand : IRequest<ResponseResultDTO>
    {
        public int PermissionId { get; set; }
        public string PermissionCatagory { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;
        public string PermissionDescription { get; set; } = string.Empty;
    }

    public record DeletePermissionCommand(int Id) : IRequest<ResponseResultDTO>;
    #endregion

    #region Validators
    public class CreatePermissionValidator : AbstractValidator<CreatePermissionCommand>
    {
        public CreatePermissionValidator()
        {
            RuleFor(x => x.PermissionCatagory).NotEmpty().MaximumLength(50);
            RuleFor(x => x.PermissionName).NotEmpty().MaximumLength(80);
            RuleFor(x => x.PermissionDescription).NotEmpty().MaximumLength(100);
        }
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
    #endregion

    #region Handlers - Updated to Return ResponseResultDTO

    // --- Get All ---
    public class GetAllPermissionsHandler(DBContextHRsystem db) : IRequestHandler<GetAllPermissionsQuery, ResponseResultDTO<List<AspPermission>>>
    {
        public async Task<ResponseResultDTO<List<AspPermission>>> Handle(GetAllPermissionsQuery request, CancellationToken ct)
        {
            var data = await db.AspPermissions.AsNoTracking().ToListAsync(ct);

            return new ResponseResultDTO<List<AspPermission>>
            {
                Success = true,
                Message = "Permissions retrieved successfully.",
                Data = data
            };
        }
    }

    // --- Get One ---
    public class GetPermissionByIdHandler(DBContextHRsystem db) : IRequestHandler<GetPermissionByIdQuery, ResponseResultDTO<AspPermission>>
    {
        public async Task<ResponseResultDTO<AspPermission>> Handle(GetPermissionByIdQuery request, CancellationToken ct)
        {
            var entity = await db.AspPermissions.AsNoTracking()
                .FirstOrDefaultAsync(x => x.PermissionId == request.Id, ct);

            if (entity == null)
            {
                return new ResponseResultDTO<AspPermission>
                {
                    Success = false,
                    Message = $"Permission with ID {request.Id} not found."
                };
            }

            return new ResponseResultDTO<AspPermission>
            {
                Success = true,
                Message = "Permission retrieved successfully.",
                Data = entity
            };
        }
    }

    // --- Create ---
    public class CreatePermissionHandler(DBContextHRsystem db, ICurrentUserService userService)
        : IRequestHandler<CreatePermissionCommand, ResponseResultDTO>
    {
        public async Task<ResponseResultDTO> Handle(CreatePermissionCommand request, CancellationToken ct)
        {
            var exists = await db.AspPermissions.AnyAsync(x => x.PermissionName == request.PermissionName, ct);

            if (exists)
            {
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = $"A permission with the name '{request.PermissionName}' already exists."
                };
            }

            var entity = new AspPermission
            {
                PermissionCatagory = request.PermissionCatagory,
                PermissionName = request.PermissionName,
                PermissionDescription = request.PermissionDescription,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userService.UserId // Assumes UserId is available
            };

            db.AspPermissions.Add(entity);
            await db.SaveChangesAsync(ct);

            return new ResponseResultDTO
            {
                Success = true,
                Message = $"Permission '{request.PermissionName}' created successfully."
            };
            // Note: If you need to return the ID, change IRequest<ResponseResultDTO> to IRequest<ResponseResultDTO<int>>
        }
    }

    // --- Update ---
    public class UpdatePermissionHandler(DBContextHRsystem db, ICurrentUserService userService)
        : IRequestHandler<UpdatePermissionCommand, ResponseResultDTO>
    {
        public async Task<ResponseResultDTO> Handle(UpdatePermissionCommand request, CancellationToken ct)
        {
            var entity = await db.AspPermissions.FindAsync(new object?[] { request.PermissionId }, ct);

            if (entity == null)
                return new ResponseResultDTO { Success = false, Message = $"Permission with ID {request.PermissionId} not found." };


            // check duplicate name
            var exists = await db.AspPermissions
                .AnyAsync(x => x.PermissionName == request.PermissionName && x.PermissionId != request.PermissionId, ct);

            if (exists)
                return new ResponseResultDTO { Success = false, Message = $"A permission with the name '{request.PermissionName}' already exists." };

            // Update fields
            entity.PermissionCatagory = request.PermissionCatagory;
            entity.PermissionName = request.PermissionName;
            entity.PermissionDescription = request.PermissionDescription;
           // entity.CreatedAt = DateTime.UtcNow;
           // entity.UpdatedBy = userService.UserId; // Assuming you have UpdatedBy field/logic

            await db.SaveChangesAsync(ct);

            return new ResponseResultDTO { Success = true, Message = $"Permission with ID {request.PermissionId} updated successfully." };
        }
    }

    // --- Delete ---
    public class DeletePermissionHandler(DBContextHRsystem db)
        : IRequestHandler<DeletePermissionCommand, ResponseResultDTO>
    {
        public async Task<ResponseResultDTO> Handle(DeletePermissionCommand request, CancellationToken ct)
        {
            var entity = await db.AspPermissions.FindAsync(new object?[] { request.Id }, ct);

            if (entity == null)
                return new ResponseResultDTO { Success = false, Message = $"Permission with ID {request.Id} not found." };

            // NOTE: Consider adding a check here for role assignments before deleting the permission.
            // If the permission is assigned to roles, you might return a Conflict (409) error.

            db.AspPermissions.Remove(entity);
            await db.SaveChangesAsync(ct);

            return new ResponseResultDTO { Success = true, Message = $"Permission with ID {request.Id} deleted successfully." };
        }
    }
    #endregion
}