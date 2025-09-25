using HRsystem.Api.Features.Scheduling.RemoteWorkdays.CreateRemoteWorkday;
using HRsystem.Api.Features.Scheduling.RemoteWorkdays.DeleteRemoteWorkDays;
using HRsystem.Api.Features.Scheduling.RemoteWorkdays.GetAllRemoteWorkdays;
using HRsystem.Api.Features.Scheduling.RemoteWorkdays.GetRemoteWorkDaysById;
using HRsystem.Api.Features.Scheduling.RemoteWorkdays.UpdateRemoteWorkDays;
using MediatR;

namespace HRsystem.Api.Features.Scheduling.RemoteWorkdays
{
    public static class RemoteWorkDaysEndpoints
    {
        public static void MapRemoteWorkDaysEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Scheduling/RemoteWorkDays").WithTags("RemoteWorkDays");

            // Get all
            group.MapGet("/GetList", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllRemoteWorkDaysQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/Create", async (ISender mediator, CreateRemoteWorkDaysCommand command) =>
            {
                var id = await mediator.Send(command);
                return Results.Ok(new { Success = true, Id = id });
            });

            // Update
            group.MapPut("/UpdateOne/{id}", async (ISender mediator, int id, UpdateRemoteWorkDaysCommand command) =>
            {
                if (id != command.Id) return Results.BadRequest();
                var success = await mediator.Send(command);
                return success ? Results.Ok(new { Success = true }) : Results.NotFound();
            });

            // Delete
            group.MapDelete("/DeleteOne/{id}", async (ISender mediator, int id) =>
            {
                var success = await mediator.Send(new DeleteRemoteWorkDaysCommand(id));
                return success ? Results.Ok(new { Success = true }) : Results.NotFound();
            });

            group.MapGet("/GetOne/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetRemoteWorkDaysByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"RemoteWorkDays {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

        }
    }
}
