using MediatR;

namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
    public static class EmployeeAppEndPoints
    {
        public static void MapEmployeeAppEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/employee-App").WithTags("Employee App");

            // Get pending activities for current user
            group.MapGet("/EmployeeInfo", async (ISender mediator) =>
            {
                var result = await mediator.Send(new EmployeeInfoQueury());
                if (result == null)
                    return Results.NotFound(new { Success = false, Message = "No Employee  found" });

                return Results.Ok(new { Success = true, Data = result });
            });

            group.MapGet("/AnnualBalance", async (ISender mediator) =>
            {
                var result = await mediator.Send(new EmployeeAnnualBalance());
                if (result == null)
                    return Results.NotFound(new { Success = false, Message = "No Employee  found" });

                return Results.Ok(new { Success = true, Data = result });
            });

            group.MapGet("/CasualBalance", async (ISender mediator) =>
            {
                var result = await mediator.Send(new EmployeeCasualBalance());
                if (result == null)
                    return Results.NotFound(new { Success = false, Message = "No Employee  found" });

                return Results.Ok(new { Success = true, Data = result });
            });
        }
    }
}
