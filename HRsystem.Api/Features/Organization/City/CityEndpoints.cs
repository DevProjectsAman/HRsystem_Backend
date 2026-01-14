using FluentValidation;
 
using HRsystem.Api.Features.Organization.City.CreateCity;
using HRsystem.Api.Features.Organization.City.DeleteCity;
using HRsystem.Api.Features.Organization.City.GetAllCities;
using HRsystem.Api.Features.Organization.City.GetCityByGovId;
using HRsystem.Api.Features.Organization.City.GetCityById;
using HRsystem.Api.Features.Organization.City.UpdateCity;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace HRsystem.Api.Features.Organization.City
{
    public static class CityEndpoints
    {
        public static void MapCityEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Organization/cities").WithTags("Cities");

            // Get All
            group.MapGet("/ListOfCities", [Authorize] async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllCitiesQuery());
                return Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });

            // Get One
            group.MapGet("/GetOneCity/{id}", [Authorize] async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetCityByCityIdQuery(id));
                return result == null
                    ? Results.NotFound(new ResponseResultDTO { Success = false, Message = $"City {id} not found" })
                    : Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });

            // Get One By GovID
            group.MapGet("/GetOneCityByGov/{GovId}", [Authorize] async (int GovId, ISender mediator) =>
            {
                var result = await mediator.Send(new GetCityByGovIdQuery(GovId));
                return result == null
                    ? Results.NotFound(new ResponseResultDTO { Success = false, Message = $"City {GovId} not found" })
                    : Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateCity", [Authorize] async (CreateCityCommand cmd, ISender mediator, IValidator<CreateCityCommand> validator) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
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
                return Results.Created($"/api/Organization/cities/{result.CityId}", new ResponseResultDTO<object>
                {
                    Success = true,
                    Data = result,
                    Message = "Created"
                });
            });

            // Update
            group.MapPut("/UpdateCity/{id}", [Authorize] async (int id, UpdateCityCommand cmd, ISender mediator, IValidator<UpdateCityCommand> validator) =>
            {
                if (id != cmd.CityId)
                    return Results.BadRequest(new ResponseResultDTO { Success = false, Message = "Id mismatch" });

                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
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
                    ? Results.NotFound(new ResponseResultDTO { Success = false, Message = $"City {id} not found" })
                    : Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/DeleteCity/{id}", [Authorize] async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteCityCommand(id));
                return !result
                    ? Results.NotFound(new ResponseResultDTO { Success = false, Message = $"City {id} not found" })
                    : Results.Ok(new ResponseResultDTO { Success = true, Message = $"City {id} deleted successfully" });
            });
        }
    }
}
