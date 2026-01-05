using HRsystem.Api.Features.Scheduling.WorkDaysRules.GetAllWorkDaysRules;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HRsystem.Api.Features.Scheduling.WorkDaysRules
{
    public static class WorkDaysRuleEndpoints
    {
        public static void MapWorkDaysRuleEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Scheduling/workdaysrules").WithTags("WorkDaysRules");

            // Get All
            group.MapGet("/GetListOfWorkDaysRule/{CompanyId}", [Authorize] async (ISender mediator,int CompanyId) =>
            {
                var result = await mediator.Send(new GetAllWorkDaysRulesQuery(CompanyId));
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get One
            group.MapGet("/GetOneOfWorkDaysRule/{id}", [Authorize] async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetWorkDaysRuleByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"WorkDaysRule {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateWorkDaysRule", [Authorize] async (CreateWorkDaysRuleCommand cmd, ISender mediator) =>
            {
                var result = await mediator.Send(cmd);
                return Results.Created($"/api/workdaysrules/{result.WorkDaysRuleId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/UpdateOneOfWorkDaysRule", [Authorize] async ( UpdateWorkDaysRuleCommand cmd, ISender mediator) =>
            {
                 

                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"WorkDaysRule {cmd.WorkDaysRuleId} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/DeleteOneOfWorkDaysRule/{id}", [Authorize] async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteWorkDaysRuleCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"WorkDaysRule {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"WorkDaysRule {id} deleted successfully" });
            });
        }
    }
}
