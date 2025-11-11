using HRsystem.Api.Features.WorkDaysRules;
using HRsystem.Api.Features.WorkDaysRules.GetAllWorkDaysRules;
using MediatR;

namespace HRsystem.Api.Features.WorkDaysRules
{
    public static class WorkDaysRuleEndpoints
    {
        public static void MapWorkDaysRuleEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Scheduling/workdaysrules").WithTags("WorkDaysRules");

            // Get All
            group.MapGet("/GetListOfWorkDaysRule/{CompanyId}", async (ISender mediator,int CompanyId) =>
            {
                var result = await mediator.Send(new GetAllWorkDaysRulesQuery(CompanyId));
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get One
            group.MapGet("/GetOneOfWorkDaysRule/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetWorkDaysRuleByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"WorkDaysRule {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateWorkDaysRule", async (CreateWorkDaysRuleCommand cmd, ISender mediator) =>
            {
                var result = await mediator.Send(cmd);
                return Results.Created($"/api/workdaysrules/{result.WorkDaysRuleId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/UpdateOneOfWorkDaysRule/{id}", async (int id, UpdateWorkDaysRuleCommand cmd, ISender mediator) =>
            {
                if (id != cmd.WorkDaysRuleId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"WorkDaysRule {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/DeleteOneOfWorkDaysRule/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteWorkDaysRuleCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"WorkDaysRule {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"WorkDaysRule {id} deleted successfully" });
            });
        }
    }
}
