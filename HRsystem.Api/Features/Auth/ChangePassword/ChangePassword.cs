using MediatR;
using FluentValidation;

namespace HRsystem.Api.Features.Auth.ChangePassword;

public class ChangePasswordCommand : IRequest<ChangePasswordResponse>
{
    public string UserName { get; set; } = default!;
    public string CurrentPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}

public record ChangePasswordResponse(bool Success, string Message);

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.CurrentPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
    }
}
