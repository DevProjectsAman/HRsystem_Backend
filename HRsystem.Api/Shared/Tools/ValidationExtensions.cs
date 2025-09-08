using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace HRsystem.Api.Shared.Tools
{
    using FluentValidation;
    using Microsoft.AspNetCore.Http;

    public static class ValidationExtensions
    {
        public static async Task<IResult> ValidateRequest<T>(
            this T request,
            IValidator<T> validator)
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => new
                    {
                        Field = e.PropertyName,
                        Error = e.ErrorMessage
                    });

                return Results.BadRequest(new
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            return null!; // means valid
        }
    }


}

