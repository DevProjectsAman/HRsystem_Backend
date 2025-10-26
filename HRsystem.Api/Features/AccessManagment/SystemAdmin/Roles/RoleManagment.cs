using FluentValidation;
using global::HRsystem.Api.Shared.DTO;
using HRsystem.Api.Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace HRsystem.Api.Features.AccessManagment.SystemAdmin.Roles
{
    public static class AspRolesEndpoints
    {
        public static void MapAspRoleEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/SystemAdmin").WithTags("_RoleManagement");

            // List roles (optional search)
            group.MapGet("/ListRoles", [Authorize] async (IMediator mediator, string? q) =>
                await mediator.Send(new ListRolesQuery(q)))
                .WithName("ListRoles");

            // Get role by id
            group.MapGet("/GetRole/{roleId}", [Authorize] async (IMediator mediator, int roleId) =>
                await mediator.Send(new GetRoleQuery(roleId)))
                .WithName("GetRole");

            // Create role
            group.MapPost("/CreateRole", [Authorize] async (IMediator mediator, CreateRoleCommand cmd) =>
                await mediator.Send(cmd))
                .WithName("CreateRole");

            // Update role
            group.MapPut("/UpdateRole", [Authorize] async (IMediator mediator, UpdateRoleCommand cmd) =>
            {
                return await mediator.Send(cmd);
            })
            .WithName("UpdateRole");

            // Delete role
            group.MapDelete("/DeleteRole/{roleId}", [Authorize] async (IMediator mediator, string roleId) =>
                await mediator.Send(new DeleteRoleCommand(roleId)))
                .WithName("DeleteRole");
        }
    }

    #region DTOs
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
    }
    #endregion

    #region Requests (Queries & Commands)
    public record ListRolesQuery(string? Search = null) : IRequest<ResponseResultDTO<List<RoleDto>>>;

    public record GetRoleQuery(int RoleId) : IRequest<ResponseResultDTO<RoleDto>>;

    // ✅ Updated: Flat structure like CreateDepartmentCommand
    public record CreateRoleCommand(string RoleName) : IRequest<ResponseResultDTO<RoleDto>>;

    // ✅ Updated: Flat structure
    public record UpdateRoleCommand(int RoleId, string RoleName) : IRequest<ResponseResultDTO<RoleDto>>;

    public record DeleteRoleCommand(string RoleId) : IRequest<ResponseResultDTO<bool>>;
    #endregion

    #region Validators
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
    #endregion

    #region Handlers
    // List
    public class ListRolesHandler : IRequestHandler<ListRolesQuery, ResponseResultDTO<List<RoleDto>>>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ListRolesHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ResponseResultDTO<List<RoleDto>>> Handle(ListRolesQuery request, CancellationToken ct)
        {
            var query = _roleManager.Roles.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var s = request.Search.Trim().ToUpperInvariant();
                query = query.Where(r => r.Name.ToUpper().Contains(s) || r.NormalizedName.Contains(s));
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
                Message = "Roles retrieved successfully",
                Data = list
            };
        }
    }

    // Get
    public class GetRoleHandler : IRequestHandler<GetRoleQuery, ResponseResultDTO<RoleDto>>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public GetRoleHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ResponseResultDTO<RoleDto>> Handle(GetRoleQuery request, CancellationToken ct)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
            if (role == null)
            {
                return new ResponseResultDTO<RoleDto>
                {
                    Success = false,
                    Message = "Role not found",
                    Data = null!
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
                Message = "Role found",
                Data = dto
            };
        }
    }

    // Create
    public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, ResponseResultDTO<RoleDto>>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public CreateRoleHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ResponseResultDTO<RoleDto>> Handle(CreateRoleCommand request, CancellationToken ct)
        {
            // ✅ Updated: Use RoleName directly
            var role = new ApplicationRole(request.RoleName.Trim());

            var exists = await _roleManager.RoleExistsAsync(role.Name!);
            if (exists)
                return new ResponseResultDTO<RoleDto> { Success = false, Message = "Role already exists" };

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return new ResponseResultDTO<RoleDto>
                {
                    Success = false,
                    Message = string.Join("; ", result.Errors.Select(e => e.Description))
                };
            }

            var dto = new RoleDto
            {
                RoleId = role.Id,
                RoleName = role.Name!,
                NormalizedName = role.NormalizedName ?? string.Empty
            };

            return new ResponseResultDTO<RoleDto>
            {
                Success = true,
                Message = "Role created successfully",
                Data = dto
            };
        }
    }

    // Update
    public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, ResponseResultDTO<RoleDto>>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UpdateRoleHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ResponseResultDTO<RoleDto>> Handle(UpdateRoleCommand request, CancellationToken ct)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
            if (role == null)
                return new ResponseResultDTO<RoleDto> { Success = false, Message = "Role not found" };

            // ✅ Updated: Use RoleName directly
            role.Name = request.RoleName.Trim();
            role.NormalizedName = _roleManager.NormalizeKey(role.Name);

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                return new ResponseResultDTO<RoleDto>
                {
                    Success = false,
                    Message = string.Join("; ", result.Errors.Select(e => e.Description))
                };

            var dto = new RoleDto
            {
                RoleId = role.Id,
                RoleName = role.Name ?? string.Empty,
                NormalizedName = role.NormalizedName ?? string.Empty
            };

            return new ResponseResultDTO<RoleDto>
            {
                Success = true,
                Message = "Role updated successfully",
                Data = dto
            };
        }
    }

    // Delete
    public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, ResponseResultDTO<bool>>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DeleteRoleHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ResponseResultDTO<bool>> Handle(DeleteRoleCommand request, CancellationToken ct)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role == null)
                return new ResponseResultDTO<bool> { Success = false, Message = "Role not found", Data = false };

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                return new ResponseResultDTO<bool>
                {
                    Success = false,
                    Message = string.Join("; ", result.Errors.Select(e => e.Description)),
                    Data = false
                };

            return new ResponseResultDTO<bool>
            {
                Success = true,
                Message = "Role deleted successfully",
                Data = true
            };
        }
    }
    #endregion
}