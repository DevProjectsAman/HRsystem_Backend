using HRsystem.Api.Features.Scheduling.VacationRulesGroup.DTO;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HRsystem.Api.Features.Scheduling.VacationRulesGroup
{
    // Ensure you have using statements for the nested commands/queries, e.g.:
    // using HRsystem.Api.Features.Scheduling.VacationRulesGroup.Create;
    // using HRsystem.Api.Features.Scheduling.VacationRulesGroup.Get;
    // using HRsystem.Api.Features.Scheduling.VacationRulesGroup.Update;
    // using HRsystem.Api.Features.Scheduling.VacationRulesGroup.Delete;

    public static class VacationRulesGroupEndpoints
    {
        public static void MapVacationRulesGroupEndpoints(this IEndpointRouteBuilder app)
        {
            // 1. Use MapGroup and WithTags for better structure and Swagger grouping
            var group = app.MapGroup("/api/vacationrulesgroup").WithTags("VacationRulesGroup");

            // Create (POST)
            group.MapPost("/Create", async (Create.CreateVacationRulesGroupCommand cmd, ISender mediator) =>
            {
                var result = await mediator.Send(cmd);
                // Return 201 Created with the location of the new resource
               // return Results.Created($"/api/vacationrulesgroup/{result.GroupId}", new { Success = true, Data = result });
 return Results.Ok(new { Success = true, Message = "Vacation Rules Group created successfully", Data = result });
            });

            // Get All (GET)
            group.MapGet("/Get", async (ISender mediator) =>
            {
                var result = await mediator.Send(new Get.GetVacationRulesGroupsQuery());

                if (result == null || !result.Any())
                    return Results.NotFound(new { Success = false, Message = "No Vacation Rules Groups found" });

                return Results.Ok(new
                {
                    Success = true,
                    Message = "Vacation Rules Groups retrieved successfully",
                    Data = result
                });
            });

            // Get by Id (GET)
            group.MapGet("/GetById/{id:int}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new Get.GetVacationRulesGroupByIdQuery(id));

                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Vacation Rules Group {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            group.MapPost("/GetBestVacationRulesMatch/", async (GetMatchingVacationRulesQuery request, ISender mediator) =>
            {
                var result = await mediator.Send(request);

                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Vacation Rules Group for company  {request.CompanyId} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Update (PUT)
            group.MapPut("/Update/", async ( Update.UpdateVacationRulesGroupCommand cmd, ISender mediator) =>
            {
                // Optional: Check for Id mismatch if the command class contains the Id
                
                var result = await mediator.Send(cmd );

                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Vacation Rules Group {cmd.GroupId} not found or update failed" })
                    : Results.Ok(new { Success = true, Message = $"Vacation Rules Group {cmd.GroupId} updated successfully", Data = result });
            });

            // Delete (DELETE)
            group.MapDelete("/Delete/{id:int}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new Delete.DeleteVacationRulesGroupCommand(id));

                // Assuming the handler returns a boolean indicating successful deletion
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Vacation Rules Group {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Vacation Rules Group {id} deleted successfully" });
            });
        }
    }
}