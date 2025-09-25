using HRsystem.Api.Features.ShiftRule.CreateShiftRule;
using HRsystem.Api.Features.ShiftRule.GetAllShiftRules;
using HRsystem.Api.Features.ShiftRule.GetShiftRuleById;
using HRsystem.Api.Features.ShiftRule.UpdateShiftRule;
using HRsystem.Api.Features.ShiftRule.DeleteShiftRule;
using MediatR;
using FluentValidation;
using HRsystem.Api.Features.ShiftRule.GetShiftRuleByParameters;

namespace HRsystem.Api.Features.ShiftRule
{
    public static class ShiftRuleEndpoints
    {
        public static void MapShiftRuleEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Lookups/shiftrules").WithTags("ShiftRules");

            // Get all
            group.MapGet("/ListShiftRules", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllShiftRulesQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            group.MapGet("/GetOneShiftRule/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetShiftRuleByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Shift Rule {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // ✅ NEW: Get by parameters (progressive filter)
            group.MapPost("/GetMatchingShiftRules", async (
                GetMatchingShiftRulesQuery query,
                ISender mediator) =>
            {
                var result = await mediator.Send(query);
                return result.Success
                    ? Results.Ok(result)
                    : Results.NotFound(result);
            });
            // Create
            group.MapPost("/CreateShiftRule", async (CreateShiftRuleCommand command, ISender mediator, IValidator<CreateShiftRuleCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return Results.Created($"/api/shiftrules/{result.RuleId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/UpdateShiftRule/{id}", async (int id, UpdateShiftRuleCommand command, ISender mediator, IValidator<UpdateShiftRuleCommand> validator) =>
            {
                if (id != command.RuleId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Shift Rule {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/DeleteShiftRule/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteShiftRuleCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Shift Rule {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Shift Rule {id} deleted successfully" });
            });
        }
    }
}
