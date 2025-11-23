using FluentValidation;
using FluentValidation.Results;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
 

namespace HRsystem.Api.Features.AccessManagment.SystemAdmin.Roles
{
    public static class AspRolesEndpoints
    {
        // Helper to standardize HTTP result returns based on DTO content
        private static IResult HandleCommandResult(ResponseResultDTO result, int? successStatusCode = 200)
        {
            if (result.Success)
            {
                // Typically 201 Created for POST, 200 OK for PUT/DELETE success
                return successStatusCode == 201
                    ? Results.Created(string.Empty, result)
                    : Results.Ok(result);
            }
            else if (result.Message.Contains("not found"))
            {
                return Results.NotFound(result); // 404
            }
            else if (result.Message.Contains("Cannot delete role"))
            {
                return Results.Conflict(result); // 409 Conflict (Business Rule Violation)
            }
            else
            {
                return Results.BadRequest(result); // 400 Bad Request
            }
        }

        // Helper to handle Validation check
        //private static IResult HandleValidationFailure<TCommand>(ValidationResult validationResult)
        //{
        //    return Results.BadRequest(new ResponseResultDTO
        //    {
        //        Success = false,
        //        Message = "Validation failed",
        //        Errors = validationResult.Errors.Select(e => new ResponseErrorDTO(e.PropertyName, e.ErrorMessage)).ToList()
        //    });
        //}

        // Original (Error-causing line 46):
        // Errors = validationResult.Errors.Select(e => new ResponseErrorDTO { Property = e.PropertyName, Error = e.ErrorMessage }).ToList()

        // Corrected: Use the primary constructor syntax (Parentheses)
        private static IResult HandleValidationFailure<TCommand>(ValidationResult validationResult)
        {
            return Results.BadRequest(new ResponseResultDTO
            {
                Success = false,
                Message = "Validation failed",
                // CORRECTED: Use object initializer syntax {}
                Errors = validationResult.Errors.Select(e => new ResponseErrorDTO
                {
                    Property = e.PropertyName,
                    Error = e.ErrorMessage
                }).ToList()
            });
        
        }

        public static void MapAspRoleEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/SystemAdmin").WithTags("_RoleManagement");

            // --- List roles (GET) ---
            group.MapGet("/ListRoles", [Authorize] async (IMediator mediator, string? q) =>
            {
                var result = await mediator.Send(new ListRolesQuery(q));
                return result.Success
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            })
            .WithName("ListRoles");

            // --- Get role by id (GET) ---
            group.MapGet("/GetRole/{roleId}", [Authorize] async (IMediator mediator, int roleId) =>
            {
                var result = await mediator.Send(new GetRoleQuery(roleId));
                return result.Success
                    ? Results.Ok(result)
                    : Results.NotFound(result); // Returns 404 if handler returns Success=false
            })
            .WithName("GetRole");

            // --- Create role (POST) ---
            group.MapPost("/CreateRole", [Authorize] async (IMediator mediator, IValidator<CreateRoleCommand> validator, CreateRoleCommand cmd) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                {
                    return HandleValidationFailure<CreateRoleCommand>(validationResult);
                }

                var result = await mediator.Send(cmd);
                return HandleCommandResult(result, successStatusCode: 201);
            })
            .WithName("CreateRole");

            // --- Update role (PUT) ---
            group.MapPut("/UpdateRole", [Authorize] async (IMediator mediator, IValidator<UpdateRoleCommand> validator, UpdateRoleCommand cmd) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                {
                    return HandleValidationFailure<UpdateRoleCommand>(validationResult);
                }

                var result = await mediator.Send(cmd);
                return HandleCommandResult(result);
            })
            .WithName("UpdateRole");

            // --- Delete role (DELETE) ---
            group.MapDelete("/DeleteRole/{roleId}", [Authorize] async (IMediator mediator, string roleId) =>
            {
                var result = await mediator.Send(new DeleteRoleCommand(roleId));
                return HandleCommandResult(result); // Handles 404 (not found) and 409 (users assigned)
            })
            .WithName("DeleteRole");
        }
    }

    // ===================================================================
    // 2. DTOs
    // ===================================================================

    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
    }

    // ===================================================================
    // 3. REQUESTS (Queries & Commands) - Updated to return DTOs
    // ===================================================================

    public record ListRolesQuery(string? Search = null) : IRequest<ResponseResultDTO<List<RoleDto>>>;

    public record GetRoleQuery(int RoleId) : IRequest<ResponseResultDTO<RoleDto>>;

    // Commands returning only ResponseResultDTO for success/failure info
    public record CreateRoleCommand(string RoleName) : IRequest<ResponseResultDTO>;

    public record UpdateRoleCommand(int RoleId, string RoleName) : IRequest<ResponseResultDTO>;

    public record DeleteRoleCommand(string RoleId) : IRequest<ResponseResultDTO>;

    // ===================================================================
    // 4. VALIDATORS
    // ===================================================================

    public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required")
                .MaximumLength(256).WithMessage("Role name cannot exceed 256 characters");
        }
    }

    public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleValidator()
        {
            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("Role ID must be greater than 0");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required")
                .MaximumLength(256).WithMessage("Role name cannot exceed 256 characters");
        }
    }

    public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role ID is required");
        }
    }

    // ===================================================================
    // 5. HANDLERS - Updated to return ResponseResultDTOs
    // ===================================================================

    // --- List ---
    public class ListRolesHandler : IRequestHandler<ListRolesQuery, ResponseResultDTO<List<RoleDto>>>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ListRolesHandler(RoleManager<ApplicationRole> roleManager) => _roleManager = roleManager;

        public async Task<ResponseResultDTO<List<RoleDto>>> Handle(ListRolesQuery request, CancellationToken ct)
        {
            var query = _roleManager.Roles.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var s = request.Search.Trim().ToUpperInvariant();
                query = query.Where(r => r.Name!.ToUpper().Contains(s) || r.NormalizedName!.Contains(s));
            }

            var list = await query
                .OrderBy(r => r.Name)
                .Select(r => new RoleDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name ?? string.Empty,
                    NormalizedName = r.NormalizedName ?? string.Empty
                })
                .ToListAsync(ct);

            return new ResponseResultDTO<List<RoleDto>>
            {
                Success = true,
                Message = "Roles retrieved successfully.",
                Data = list
            };
        }
    }

    // --- Get ---
    public class GetRoleHandler : IRequestHandler<GetRoleQuery, ResponseResultDTO<RoleDto>>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public GetRoleHandler(RoleManager<ApplicationRole> roleManager) => _roleManager = roleManager;

        public async Task<ResponseResultDTO<RoleDto>> Handle(GetRoleQuery request, CancellationToken ct)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

            if (role == null)
            {
                return new ResponseResultDTO<RoleDto>
                {
                    Success = false,
                    Message = $"Role with ID {request.RoleId} not found."
                };
            }

            var dto = new RoleDto
            {
                RoleId = role.Id,
                RoleName = role.Name ?? string.Empty,
                NormalizedName = role.NormalizedName ?? string.Empty
            };

            return new ResponseResultDTO<RoleDto>
            {
                Success = true,
                Message = "Role retrieved successfully.",
                Data = dto
            };
        }
    }

    // --- Create ---
    public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, ResponseResultDTO>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public CreateRoleHandler(RoleManager<ApplicationRole> roleManager) => _roleManager = roleManager;

        public async Task<ResponseResultDTO> Handle(CreateRoleCommand request, CancellationToken ct)
        {
            var roleName = request.RoleName.Trim();
            var exists = await _roleManager.RoleExistsAsync(roleName);

            if (exists)
            {
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = $"Role name '{roleName}' already exists."
                };
            }

            var role = new ApplicationRole(roleName);
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = "Role creation failed.",
                    Errors = result.Errors.Select(e => new ResponseErrorDTO {Property= e.Code,Error= e.Description }).ToList()
                    
                };
            }

            return new ResponseResultDTO { Success = true, Message = $"Role '{roleName}' created successfully." };
        }
    }

    // --- Update ---
    public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, ResponseResultDTO>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UpdateRoleHandler(RoleManager<ApplicationRole> roleManager) => _roleManager = roleManager;

        public async Task<ResponseResultDTO> Handle(UpdateRoleCommand request, CancellationToken ct)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

            if (role == null)
                return new ResponseResultDTO { Success = false, Message = $"Role with ID {request.RoleId} not found." };

            role.Name = request.RoleName.Trim();
            role.NormalizedName = _roleManager.NormalizeKey(role.Name);

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = "Role update failed.",
                   // Errors = result.Errors
                };

            return new ResponseResultDTO { Success = true, Message = $"Role '{role.Name}' updated successfully." };
        }
    }

    // --- Delete ---
    public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, ResponseResultDTO>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteRoleHandler(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<ResponseResultDTO> Handle(DeleteRoleCommand request, CancellationToken ct)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId);

            if (role == null)
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = $"Role with ID '{request.RoleId}' not found."
                };

            // 1. Check for assigned users
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

            if (usersInRole.Any())
            {
                // 2. Return specific failure message if users are assigned
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = $"Cannot delete role '{role.Name}'. {usersInRole.Count} user(s) are currently assigned to it. Please reassign or delete these users first."
                };
            }

            // 3. Proceed with deletion
            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = "Role deletion failed due to an Identity server error.",
                    Errors = result.Errors.Select(e => new ResponseErrorDTO{ Property = e.Code, Error = e.Description }).ToList()
                };
            }

            return new ResponseResultDTO
            {
                Success = true,
                Message = $"Role '{role.Name}' deleted successfully."
            };
        }
    }
}