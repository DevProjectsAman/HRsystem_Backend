using FluentValidation;
using global::HRsystem.Api.Shared.DTO;
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
                    //.RequireAuthorization(policy =>
                    //    policy.RequireAssertion(ctx =>
                    //        ctx.User.IsInRole("SystemAdmin") ||
                    //        ctx.User.HasClaim("Permission", "CanViewRoles")))
                    .WithName("ListRoles");

                // Get role by id
                group.MapGet("/GetRole/{roleId}", [Authorize] async (IMediator mediator, string roleId) =>
                    await mediator.Send(new GetRoleQuery(roleId)))
                    .WithName("GetRole");

                // Create role
                group.MapPost("/CreateRole", [Authorize] async (IMediator mediator, CreateRoleCommand cmd) =>
                    await mediator.Send(cmd))
                    .WithName("CreateRole");

                // Update role
                group.MapPut("/UpdateRole", [Authorize] async (IMediator mediator, UpdateRoleCommand cmd) =>
                {
                    // ensure route id consistency
                   // cmd.RoleId = roleId;
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
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string NormalizedName { get; set; } = string.Empty;
        }

        public class CreateRoleDto
        {
            public string Name { get; set; } = string.Empty;
        }

        public class UpdateRoleDto
        {
            public string Name { get; set; } = string.Empty;
        }
        #endregion

        #region Requests (Queries & Commands)
        public record ListRolesQuery(string? Search = null) : IRequest<ResponseResultDTO<List<RoleDto>>>;

        public record GetRoleQuery(string RoleId) : IRequest<ResponseResultDTO<RoleDto>>;

        public record CreateRoleCommand(CreateRoleDto Request) : IRequest<ResponseResultDTO<RoleDto>>;

        public record UpdateRoleCommand(string RoleId, UpdateRoleDto Request) : IRequest<ResponseResultDTO<RoleDto>>
        {
            // route population helper
            public string RoleIdRoute => RoleId;
        }

        public record DeleteRoleCommand(string RoleId) : IRequest<ResponseResultDTO<bool>>;
        #endregion

        #region Validators
        public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
        {
            public CreateRoleValidator()
            {
                RuleFor(x => x.Request).NotNull();
                RuleFor(x => x.Request.Name).NotEmpty().MaximumLength(256);
            }
        }

        public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
        {
            public UpdateRoleValidator()
            {
                RuleFor(x => x.RoleId).NotEmpty();
                RuleFor(x => x.Request).NotNull();
                RuleFor(x => x.Request.Name).NotEmpty().MaximumLength(256);
            }
        }

        public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
        {
            public DeleteRoleValidator()
            {
                RuleFor(x => x.RoleId).NotEmpty();
            }
        }
        #endregion

        #region Handlers
        // List
        public class ListRolesHandler : IRequestHandler<ListRolesQuery, ResponseResultDTO<List<RoleDto>>>
        {
            private readonly RoleManager<IdentityRole> _roleManager;

            public ListRolesHandler(RoleManager<IdentityRole> roleManager)
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
                        Id = r.Id,
                        Name = r.Name ?? string.Empty,
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
            private readonly RoleManager<IdentityRole> _roleManager;

            public GetRoleHandler(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public async Task<ResponseResultDTO<RoleDto>> Handle(GetRoleQuery request, CancellationToken ct)
            {
                var role = await _roleManager.FindByIdAsync(request.RoleId);
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
                    Id = role.Id,
                    Name = role.Name ?? string.Empty,
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
            private readonly RoleManager<IdentityRole> _roleManager;

            public CreateRoleHandler(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public async Task<ResponseResultDTO<RoleDto>> Handle(CreateRoleCommand request, CancellationToken ct)
            {
                var role = new IdentityRole(request.Request.Name.Trim());

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

                var dto = new RoleDto { Id = role.Id, Name = role.Name!, NormalizedName = role.NormalizedName ?? string.Empty };

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
            private readonly RoleManager<IdentityRole> _roleManager;

            public UpdateRoleHandler(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public async Task<ResponseResultDTO<RoleDto>> Handle(UpdateRoleCommand request, CancellationToken ct)
            {
                var role = await _roleManager.FindByIdAsync(request.RoleId);
                if (role == null)
                    return new ResponseResultDTO<RoleDto> { Success = false, Message = "Role not found" };

                role.Name = request.Request.Name.Trim();
                // NormalizedName will be updated by RoleManager when UpdateAsync runs if the store implements it,
                // otherwise we can explicitly set it:
                role.NormalizedName = _roleManager.NormalizeKey(role.Name);

                var result = await _roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                    return new ResponseResultDTO<RoleDto> { Success = false, Message = string.Join("; ", result.Errors.Select(e => e.Description)) };

                var dto = new RoleDto { Id = role.Id, Name = role.Name ?? string.Empty, NormalizedName = role.NormalizedName ?? string.Empty };

                return new ResponseResultDTO<RoleDto> { Success = true, Message = "Role updated successfully", Data = dto };
            }
        }

        // Delete
        public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, ResponseResultDTO<bool>>
        {
            private readonly RoleManager<IdentityRole> _roleManager;

            public DeleteRoleHandler(RoleManager<IdentityRole> roleManager)
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
                    return new ResponseResultDTO<bool> { Success = false, Message = string.Join("; ", result.Errors.Select(e => e.Description)), Data = false };

                return new ResponseResultDTO<bool> { Success = true, Message = "Role deleted successfully", Data = true };
            }
        }
        #endregion
    }

