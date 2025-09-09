using HRsystem.Api.Features.Company.CreateCompany;
using MediatR;

namespace HRsystem.Api.Features.Company
{
    public static class CompanyEndpoints
    {
        public static void MapCompanyEndpoints(this IEndpointRouteBuilder app)
        {
            // Get all
            app.MapGet("/api/companies", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllCompanyCommand());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            app.MapGet("/api/companies/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetCompanyByIdCommand(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Company {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            app.MapPost("/api/companies", async (CreateCompanyCommand command, ISender mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/companies/{result.CompanyId}", new { Success = true, Data = result });
            });

            // Update
            app.MapPut("/api/companies/{id}", async (int id, UpdateCompanyCommand command, ISender mediator) =>
            {
                if (id != command.CompanyId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var result = await mediator.Send(command);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Company {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            app.MapDelete("/api/companies/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteCompanyCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Company {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Company {id} deleted successfully" });
            });
        }
    }

}
