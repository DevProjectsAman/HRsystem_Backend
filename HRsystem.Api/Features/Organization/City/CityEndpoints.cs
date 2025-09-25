using HRsystem.Api.Features.City.CreateCity;
using HRsystem.Api.Features.City.UpdateCity;
using HRsystem.Api.Features.City.GetAllCities;
using HRsystem.Api.Features.City.GetCityById;
using HRsystem.Api.Features.City.DeleteCity;
using MediatR;
using FluentValidation;

namespace HRsystem.Api.Features.City
{
    public static class CityEndpoints
    {
        public static void MapCityEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Organization/cities").WithTags("Cities");

            // Get All
            group.MapGet("/ListOfCities", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllCitiesQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get One
            group.MapGet("/GetOneCity/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetCityByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"City {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateCity", async (CreateCityCommand cmd, ISender mediator, IValidator<CreateCityCommand> validator) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new
                    {
                        Success = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage)
                    });

                var result = await mediator.Send(cmd);
                return Results.Created($"/api/cities/{result.CityId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/UpdateCity/{id}", async (int id, UpdateCityCommand cmd, ISender mediator, IValidator<UpdateCityCommand> validator) =>
            {
                if (id != cmd.CityId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new
                    {
                        Success = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage)
                    });

                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"City {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/DeleteCity/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteCityCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"City {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"City {id} deleted successfully" });
            });
        }
    }
}
