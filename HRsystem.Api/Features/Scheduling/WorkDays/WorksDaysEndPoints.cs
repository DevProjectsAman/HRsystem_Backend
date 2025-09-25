using HRsystem.Api.Features.Scheduling.WorkDays;
using MediatR;

public static class WorkDaysEndpoints
{
    public static void MapWorkDaysEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Scheduling/workdays").WithTags("WorkDays");

        group.MapGet("/GetListOfWorkDays", async (ISender mediator) =>
        {
            var result = await mediator.Send(new GetAllWorkDaysQuery());
            return Results.Ok(new { Success = true, Data = result });
        });

        group.MapGet("/GetOneOfWorkDays/{id}", async (int id, ISender mediator) =>
        {
            var result = await mediator.Send(new GetWorkDaysByIdQuery(id));
            return result == null
                ? Results.NotFound(new { Success = false, Message = "Not found" })
                : Results.Ok(new { Success = true, Data = result });
        });

        group.MapPost("/CreateWorkDays", async (CreateWorkDaysCommand cmd, ISender mediator) =>
        {
            var id = await mediator.Send(cmd);
            return Results.Ok(new { Success = true, Id = id });
        });

        group.MapPut("/UpdateOneOfWorkDays/{id}", async (int id, UpdateWorkDaysCommand cmd, ISender mediator) =>
        {
            if (id != cmd.Id) return Results.BadRequest();
            var updated = await mediator.Send(cmd);
            return updated ? Results.Ok(new { Success = true }) : Results.NotFound();
        });

        group.MapDelete("/DeleteOneOfWorkDays/{id}", async (int id, ISender mediator) =>
        {
            var deleted = await mediator.Send(new DeleteWorkDaysCommand(id));
            return deleted ? Results.Ok(new { Success = true }) : Results.NotFound();
        });
    }
}
