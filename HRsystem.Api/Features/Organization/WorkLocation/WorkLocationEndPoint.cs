using FluentValidation;
using HRsystem.Api.Features.Organization.WorkLocation.CreateWorkLocation;
using HRsystem.Api.Features.Organization.WorkLocation.DeleteWorkLocation;
using HRsystem.Api.Features.Organization.WorkLocation.GetAllWorkLocations;
using HRsystem.Api.Features.Organization.WorkLocation.GetSpecificWorkLocations;
using HRsystem.Api.Features.Organization.WorkLocation.GetWorkLocationById;
using HRsystem.Api.Features.Organization.WorkLocation.UpdateWorkLocation;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
 

namespace HRsystem.Api.Features.Organization.WorkLocation
{
    public static class WorkLocationEndpoints
    {
        public static void MapWorkLocationEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Organization/WorkLocation").WithTags("WorkLocations");

            // Get all
            group.MapGet("/List", [Authorize] async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllWorkLocationsQuery());
                return Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });

            // Get all
            group.MapGet("/ListSpecific", [Authorize] async (ISender mediator,int companyId,int govId, int cityId) =>
            {
                var result = await mediator.Send(new GetSpecificWorkLocationsQuery( companyId , govId, cityId));
                return Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });
            // Get by Id
            group.MapGet("/GetOne", [Authorize] async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetWorkLocationByIdQuery(id));
                return result == null
                    ? Results.NotFound(new ResponseResultDTO { Success = false, Message = $"WorkLocation {id} not found" })
                    : Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });

            // Create
            group.MapPost("/Create", [Authorize] async (CreateWorkLocationCommand cmd, ISender mediator, IValidator<CreateWorkLocationCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(cmd);
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

                var result = await mediator.Send(cmd);
                return Results.Created($"/api/work-locations/{result.WorkLocationId}", new ResponseResultDTO<object>
                {
                    Success = true,
                    Message = "Created",
                    Data = result
                });
            });

            // Update
            group.MapPut("/Update", [Authorize] async (UpdateWorkLocationCommand cmd, ISender mediator, IValidator<UpdateWorkLocationCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(cmd);
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

                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new ResponseResultDTO { Success = false, Message = $"WorkLocation {cmd.WorkLocationId} not found" })
                    : Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });

            // Get hierarchy with selection (Gov → City → WorkLocation)
           

            // Delete
            group.MapDelete("/Delete", [Authorize] async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteWorkLocationCommand(id));
                return !result
                    ? Results.NotFound(new ResponseResultDTO { Success = false, Message = $"WorkLocation {id} not found" })
                    : Results.Ok(new ResponseResultDTO { Success = true, Message = $"WorkLocation {id} deleted successfully" });
            });
        }
    }
}
