using FluentValidation;
using HRsystem.Api.Features.VacationType.CreateVacationType;
using HRsystem.Api.Features.VacationType.DeleteVacationType;
using HRsystem.Api.Features.VacationType.GetAllVacationTypes;
using HRsystem.Api.Features.VacationType.GetVacationTypeById;
using HRsystem.Api.Features.VacationType.UpdateVacationType;
using MediatR;


namespace HRsystem.Api.Features.ShiftEndpoints
{
    public static class ShiftEndpoints
    {
        public static void MapVacationTypeEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Lookups/vacationtypes").WithTags("VacationTypes");

            // Get all
            group.MapGet("/ListOfVacation", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllVacationTypesQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            group.MapGet("/GetOneVacation/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetVacationTypeByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"VacationType {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateVacation", async (CreateVacationTypeCommand command, ISender mediator, IValidator<CreateVacationTypeCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return Results.Created($"/api/vacationtypes/{result}", new { Success = true, VacationTypeId = result });
            });

            // Update
            group.MapPut("/UpdateVacation/{id}", async (int id, UpdateVacationTypeCommand command, ISender mediator, IValidator<UpdateVacationTypeCommand> validator) =>
            {
                if (id != command.VacationTypeId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                if (result == null)
                {
                    return Results.NotFound(new { Success = false, Message = "Not found" });
                }

                 return Results.Ok(new { Success = true, Message = $"VacationType {id} updated successfully" });
            });

            // Delete
            group.MapDelete("/DeleteVacation/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteVacationTypeCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"VacationType {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"VacationType {id} deleted successfully" });
            });
        }
    }
}
