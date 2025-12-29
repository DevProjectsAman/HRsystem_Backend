using FluentValidation;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Organization.Govermenet.CreateGov;
using HRsystem.Api.Features.Organization.Govermenet.DeleteGov;
using HRsystem.Api.Features.Organization.Govermenet.GetAllGovs;
using HRsystem.Api.Features.Organization.Govermenet.GetGovById;
using HRsystem.Api.Features.Organization.Govermenet.UpdateGov;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using static HRsystem.Api.Features.Organization.Govermenet.GetAllGovs.Handler;

namespace HRsystem.Api.Features.Organization.Govermenet
{
    public static class GovEndpoints
    {
        public static void MapGovEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Organization/govs").WithTags("Governments");

            // Get All
            group.MapGet("/ListOfGovs", [Authorize] async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllGovsQuery());


                // Wrap result inside ResponseResultDTO
                var response = new ResponseResultDTO<IEnumerable<GovDto>>
                {
                    Data = result,   // TotalCount will be auto-set here
                    Success = true,
                    Message = "List of governors retrieved successfully"
                };

                return Results.Ok(response);
               // return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            group.MapGet("/GetOneGov/{id}", [Authorize] async (int id, ISender mediator) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Invalid GovId"
                    });
                }

                var result = await mediator.Send(new GetGovByIdQuery(id));

                if (result == null)
                {
                    return Results.NotFound(new ResponseResultDTO
                    {
                        Success = false,
                        Message = $"Gov {id} not found"
                    });
                }

                // Success case
                var response = new ResponseResultDTO<TbGov>
                {
                    Data = result, // TotalCount will be auto-set = 1
                    Success = true,
                    Message = "Gov retrieved successfully"
                };

                return Results.Ok(response);
            });


            // Create
            group.MapPost("/CreateGov", [Authorize] async (CreateGovCommand cmd, ISender mediator, IValidator<CreateGovCommand> validator) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { Success = false, Errors = validationResult.Errors.Select(e => e.ErrorMessage) });

                var result = await mediator.Send(cmd);
                return Results.Created($"/api/govs/{result.GovId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/UpdateGov/{id}", [Authorize] async (int id, UpdateGovCommand cmd, ISender mediator, IValidator<UpdateGovCommand> validator) =>
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
            group.MapDelete("/DeleteGov/{id}", [Authorize] async (int id, ISender mediator) =>
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
