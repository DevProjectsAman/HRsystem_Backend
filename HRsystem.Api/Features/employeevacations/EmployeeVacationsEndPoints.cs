using MediatR;
//using static HRsystem.Api.Features.employeevacations.EmployeeVacations;

namespace HRsystem.Api.Features.employeevacations
{
    public static class EmployeeVacationsEndPoints
    {
        public static void MapEmployeeVacationsEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/vacations").WithTags("Vacations");

            group.MapGet("/mybalances", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetEmployeeVacationsQuery());
                return result == null || !result.Any()
                    ? Results.NotFound(new { Success = false, Message = "No balances found" })
                    : Results.Ok(new { Success = true, Data = result });
            });
        }
    }
}
    
