using HRsystem.Api.Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HRsystem.Api.Features.Auth.ChangePassword;

public static class ChangePasswordEndpoint
{
    public static void MapChangePassword(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/change-password", async (
            ChangePasswordCommand command,
            ISender mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("ChangePassword")
        .WithTags("Auth")
        .WithOpenApi();
    }
}

public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangePasswordHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ChangePasswordResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null)
            return new(false, "User not found");

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        return result.Succeeded
            ? new(true, "Password changed successfully.")
            : new(false, string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}
