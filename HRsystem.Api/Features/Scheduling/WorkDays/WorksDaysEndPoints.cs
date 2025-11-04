using System.Linq;
using HRsystem.Api.Features.Scheduling.WorkDays;
using HRsystem.Api.Shared.DTO;
using MediatR;

public static class WorkDaysEndpoints
{
    public static void MapWorkDaysEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Scheduling/workdays").WithTags("WorkDays");

        group.MapGet("/GetListOfWorkDays", async (ISender mediator) =>
        {
            var result = await mediator.Send(new GetAllWorkDaysQuery());
            return Results.Ok(new ResponseResultDTO<object>
            {
                Success = true,
                Data = result
            });
        });

        group.MapGet("/GetOneOfWorkDays/{id}", async (int id, ISender mediator) =>
        {
            var result = await mediator.Send(new GetWorkDaysByIdQuery(id));
            return Results.Ok(result == null
                ? new ResponseResultDTO<object?> { Success = false, Message = "Not found"   }
                : new ResponseResultDTO<object> { Success = true, Data = result });
        });

        group.MapPost("/CreateWorkDays", async (CreateWorkDaysCommand cmd, ISender mediator) =>
        {
            var id = await mediator.Send(cmd);
            return Results.Ok(new ResponseResultDTO<int>
            {
                Success = true,
                Message = "Created",
                Data = id
            });
        });

        group.MapPut("/UpdateOneOfWorkDays/{id}", async (int id, UpdateWorkDaysCommand cmd, ISender mediator) =>
        {
            if (id != cmd.Id)
                return Results.Ok(new ResponseResultDTO { Success = false, Message = "Invalid id" });

            var updated = await mediator.Send(cmd);
            return Results.Ok(updated
                ? new ResponseResultDTO { Success = true, Message = "Updated" }
                : new ResponseResultDTO { Success = false, Message = "Not found" });
        });

        group.MapDelete("/DeleteOneOfWorkDays/{id}", async (int id, ISender mediator) =>
        {
            var deleted = await mediator.Send(new DeleteWorkDaysCommand(id));
            return Results.Ok(deleted
                ? new ResponseResultDTO { Success = true, Message = "Deleted" }
                : new ResponseResultDTO { Success = false, Message = "Not found" });
        });

        group.MapPost("/GettingWorkDaysIdByMatchingRules", async (GettingWorkDaysIdByMatchingRules query, ISender mediator) =>
        {
            var result = await mediator.Send(query);

            if (result == null || !result.Any())
                return Results.Ok(new ResponseResultDTO<object?>
                {
                    Success = false,
                    Message = "No matching work days rules found"
                });

            return Results.Ok(new ResponseResultDTO<object>
            {
                Success = true,
                Message = "Matching work days rules retrieved successfully",
                Data = result
            });
        });
    }
}
