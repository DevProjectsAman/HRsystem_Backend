using FluentValidation;
using HRsystem.Api.Features.Lookups.VacationType.CreateVacationType;
using HRsystem.Api.Features.Lookups.VacationType.DeleteVacationType;
using HRsystem.Api.Features.Lookups.VacationType.GetAllVacationTypes;
using HRsystem.Api.Features.Lookups.VacationType.GetVacationTypeById;
using HRsystem.Api.Features.Lookups.VacationType.UpdateVacationType;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;


namespace HRsystem.Api.Features.Lookups.VacationType
{
    public static class ShiftEndpoints
    {
        public static void MapVacationTypeEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Lookups/vacationtypes").WithTags("VacationTypes");

            // Get all
            group.MapGet("/ListOfVacation", [Authorize] async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllVacationTypesQuery());
                return Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });

            // Get by Id
            group.MapGet("/GetOneVacation/{id}", [Authorize] async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetVacationTypeByIdQuery(id));
                return result == null
                    ? Results.NotFound(new ResponseResultDTO { Success = false, Message = $"VacationType {id} not found" })
                    : Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateVacation", [Authorize] async (CreateVacationTypeCommand command, ISender mediator, IValidator<CreateVacationTypeCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                {
                    var errors = validation.Errors
                        .Select(e => new ResponseErrorDTO { Property = e.PropertyName, Error = e.ErrorMessage })
                        .ToList();

                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors
                    });
                }

                var result = await mediator.Send(command);
                return Results.Created($"/api/vacationtypes/{result}", new ResponseResultDTO<int>
                {
                    Success = true,
                    Message = "Created",
                    Data = result
                });
            });

            // Update
            group.MapPut("/UpdateVacation/{id}", [Authorize] async (int id, UpdateVacationTypeCommand command, ISender mediator, IValidator<UpdateVacationTypeCommand> validator) =>
            {
                if (id != command.VacationTypeId)
                    return Results.BadRequest(new ResponseResultDTO { Success = false, Message = "Id mismatch" });

                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                {
                    var errors = validation.Errors
                        .Select(e => new ResponseErrorDTO { Property = e.PropertyName, Error = e.ErrorMessage })
                        .ToList();

                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors
                    });
                }

                var result = await mediator.Send(command);
                if (result == null)
                {
                    return Results.NotFound(new ResponseResultDTO { Success = false, Message = "Not found" });
                }

                return Results.Ok(new ResponseResultDTO { Success = true, Message = $"VacationType {id} updated successfully" });
            });

            // Delete
            group.MapDelete("/DeleteVacation/{id}", [Authorize] async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteVacationTypeCommand(id));
                return !result
                    ? Results.NotFound(new ResponseResultDTO { Success = false, Message = $"VacationType {id} not found" })
                    : Results.Ok(new ResponseResultDTO { Success = true, Message = $"VacationType {id} deleted successfully" });
            });
        }
    }
}
