using HRsystem.Api.Features.Gov.CreateGov;
using HRsystem.Api.Features.Gov.UpdateGov;
using HRsystem.Api.Features.Gov.GetAllGovs;
using HRsystem.Api.Features.Gov.GetGovById;
using HRsystem.Api.Features.Gov.DeleteGov;
using MediatR;
using FluentValidation;

namespace HRsystem.Api.Features.Gov
{
    public static class GovEndpoints
    {
        public static void MapGovEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/govs").WithTags("Governments");

            // Get All
            group.MapGet("/ListOfGovs", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllGovsQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            group.MapGet("/GetOneGov/{id}", async (int id, ISender mediator) =>
            {
                if (id <= 0)
                    return Results.BadRequest(new { Success = false, Message = "Invalid GovId" });

                var result = await mediator.Send(new GetGovByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Gov {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateGov", async (CreateGovCommand cmd, ISender mediator, IValidator<CreateGovCommand> validator) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { Success = false, Errors = validationResult.Errors.Select(e => e.ErrorMessage) });

                var result = await mediator.Send(cmd);
                return Results.Created($"/api/govs/{result.GovId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/UpdateGov/{id}", async (int id, UpdateGovCommand cmd, ISender mediator, IValidator<UpdateGovCommand> validator) =>
            {
                if (id != cmd.GovId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { Success = false, Errors = validationResult.Errors.Select(e => e.ErrorMessage) });

                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Gov {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/DeleteGov/{id}", async (int id, ISender mediator) =>
            {
                if (id <= 0)
                    return Results.BadRequest(new { Success = false, Message = "Invalid GovId" });

                var result = await mediator.Send(new DeleteGovCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Gov {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Gov {id} deleted successfully" });
            });
        }
    }
}
