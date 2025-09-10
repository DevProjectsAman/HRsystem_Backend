using FluentValidation;
using HRsystem.Api.Shared.DTO;
using MediatR;

namespace HRsystem.Api.Shared.Tools;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = (await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken))))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                // Build error result with proper ResponseErrorDTO objects
                var errorMessages = failures.Select(f => f.ErrorMessage).ToList();
                var errorDetails = failures.Select(f => new ResponseErrorDTO
                {
                    Property = f.PropertyName,
                    Error = f.ErrorMessage
                }).ToList();

                // Check if TResponse is a ResponseResultDTO<T> or ResponseResultDTO
                if (typeof(TResponse).IsGenericType &&
                    typeof(TResponse).GetGenericTypeDefinition() == typeof(ResponseResultDTO<>))
                {
                    var genericType = typeof(TResponse).GetGenericArguments()[0];
                    var errorResultType = typeof(ResponseResultDTO<>).MakeGenericType(genericType);
                    var errorResult = Activator.CreateInstance(errorResultType);

                    errorResultType.GetProperty("Success")?.SetValue(errorResult, false);
                    errorResultType.GetProperty("Message")?.SetValue(errorResult, string.Join(" | ", errorMessages));
                    errorResultType.GetProperty("Errors")?.SetValue(errorResult, errorDetails);
                    errorResultType.GetProperty("Data")?.SetValue(errorResult, default);

                    return (TResponse)errorResult!;
                }
                // Handle non-generic ResponseResultDTO
                else if (typeof(TResponse) == typeof(ResponseResultDTO))
                {
                    var errorResult = new ResponseResultDTO
                    {
                        Success = false,
                        Message = string.Join(" | ", errorMessages),
                        Errors = errorDetails
                    };

                    return (TResponse)(object)errorResult;
                }

                // fallback for non-ResponseResultDTO responses
                throw new ValidationException(failures);
            }
        }

        return await next();
    }
}