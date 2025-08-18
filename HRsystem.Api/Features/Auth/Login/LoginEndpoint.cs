using FluentValidation;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Services;
using HRsystem.Api.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace HRsystem.Api.Features.Auth.Login
{
    public static class LoginEndpoint
    {
        public static void MapLogin(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/auth/login", async (
                LoginCommand command,
                ISender mediator,
                IValidator<LoginCommand> validator) =>
            {
                // ✅ Validate the command manually
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .Select(e => new { e.PropertyName, e.ErrorMessage });
                    return Results.BadRequest(errors);
                }

                // ✅ Proceed to handler
                var result = await mediator.Send(command);

                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            })
            .WithName("LoginCommand")
            .WithTags("Auth");
        }
    }

    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;

        public LoginHandler(SignInManager<ApplicationUser> signInManager,
                            UserManager<ApplicationUser> userManager,
                            JwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);
            if (!result.Succeeded)
            {
                return new LoginResponse
                {
                    Message = ResponseMessages.InvalidLogin,
                    Success = false
                };
            }

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new LoginResponse
                {
                    Message = ResponseMessages.InvalidLogin,
                    Success = false
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var jwtToken = await _jwtService.GenerateTokenAsync(user, roles);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new LoginResponse
            {
                Success = true,
                Message = ResponseMessages.SucessLogin,
                UserName = user.UserName,
                Token = tokenString
            };
        }
    }





}
