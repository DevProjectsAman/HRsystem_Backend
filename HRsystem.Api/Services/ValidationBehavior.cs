using FluentValidation;
using HRsystem.Api.Shared.DTO;
using MediatR;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
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
        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            // If response type matches our shared ResponseResultDTO pattern
            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(ResponseResultDTO<>))
            {
                var errorMessages = failures.Select(f => f.ErrorMessage).ToList();

                var errorResultType = typeof(ResponseResultDTO<>).MakeGenericType(typeof(object));
                var errorResult = Activator.CreateInstance(errorResultType);

                errorResultType.GetProperty("Success")?.SetValue(errorResult, false);
                errorResultType.GetProperty("Message")?.SetValue(errorResult, string.Join(" | ", errorMessages));
                errorResultType.GetProperty("Data")?.SetValue(errorResult, null);

                return (TResponse)errorResult!;
            }

            // fallback: still throw if response type doesn't match
            throw new ValidationException(failures);
        }

        return await next();
    }
}
