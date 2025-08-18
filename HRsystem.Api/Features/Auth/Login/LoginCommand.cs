using FluentValidation;
using MediatR;

namespace HRsystem.Api.Features.Auth.Login
{
    public record LoginCommand(string UserName, string Password) : IRequest<LoginResponse>;

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? Token { get; set; }
        public string? UserName { get; set; }
    }

    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }


}