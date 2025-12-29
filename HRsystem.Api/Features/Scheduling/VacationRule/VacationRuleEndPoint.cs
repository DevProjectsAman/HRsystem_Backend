using HRsystem.Api.Features.Scheduling.VacationRule.CreateVacationRule;
using HRsystem.Api.Features.Scheduling.VacationRule.DeleteVacationRule;
using HRsystem.Api.Features.Scheduling.VacationRule.GetAllVacationRules;
using HRsystem.Api.Features.Scheduling.VacationRule.GetVacationRuleById;
using HRsystem.Api.Features.Scheduling.VacationRule.UpdateVacationRule;
using HRsystem.Api.Features.Scheduling.WorkDays;
using HRsystem.Api.Features.ShiftRule.GetShiftRuleByParameters;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HRsystem.Api.Features.Scheduling.VacationRule
{
    public static class VacationRuleEndpoints
    {
        public static void MapVacationRuleEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Scheduling/VacationRules").WithTags("VacationRules");

            group.MapPost("/GetVacationByMatchingRules", [Authorize] async (GetVacationByMatchingRulesQueury query, ISender mediator) =>
            {
                var result = await mediator.Send(query);

                if (result == null || !result.Any())
                    return Results.NotFound(new { success = false, message = "No matching Vacations rules found" });

                return Results.Ok(new
                {
                    success = true,
                    message = "Vacations rules retrieved successfully",
                    data = result
                });
            });

            // Get all
            group.MapGet("/Listofvacationrules/{CompanyID}", [Authorize] async (int CompanyID,ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllVacationRulesQuery(CompanyID));
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            group.MapGet("/GetOnevacationrules/{id}", [Authorize] async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetVacationRuleByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Vacation Rule {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/Createvacationrules", [Authorize] async (CreateVacationRuleCommand command, ISender mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/vacationrules/{result.RuleId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/Updatevacationrules/{id}", [Authorize] async (int id, UpdateVacationRuleCommand command, ISender mediator) =>
            {
                if (id != command.RuleId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var result = await mediator.Send(command);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Vacation Rule {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/Deletevacationrules/{id}", [Authorize] async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteVacationRuleCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Vacation Rule {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Vacation Rule {id} deleted successfully" });
            });
        }
    }
}
