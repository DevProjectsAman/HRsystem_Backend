using HRsystem.Api.Features.VacationRule.GetAllVacationRules;
using HRsystem.Api.Features.VacationRule.GetVacationRuleById;
using HRsystem.Api.Features.VacationRule.DeleteVacationRule;
using MediatR;
using HRsystem.Api.Features.Scheduling.VacationRule.CreateVacationRule;
using HRsystem.Api.Features.Scheduling.VacationRule.UpdateVacationRule;

namespace HRsystem.Api.Features.Scheduling.VacationRule
{
    public static class VacationRuleEndpoints
    {
        public static void MapVacationRuleEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/vacationrules").WithTags("VacationRules");

            // Get all
            group.MapGet("/Listofvacationrules", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllVacationRulesQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            group.MapGet("/GetOnevacationrules/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetVacationRuleByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Vacation Rule {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/Createvacationrules", async (CreateVacationRuleCommand command, ISender mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/vacationrules/{result.RuleId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/Updatevacationrules/{id}", async (int id, UpdateVacationRuleCommand command, ISender mediator) =>
            {
                if (id != command.RuleId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var result = await mediator.Send(command);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Vacation Rule {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/Deletevacationrules/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteVacationRuleCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Vacation Rule {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Vacation Rule {id} deleted successfully" });
            });
        }
    }
}
