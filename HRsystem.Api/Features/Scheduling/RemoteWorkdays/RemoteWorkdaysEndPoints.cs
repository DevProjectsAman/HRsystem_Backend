using HRsystem.Api.Features.RemoteWorkdays.DeleteRemoteWorkDays;
using HRsystem.Api.Features.RemoteWorkdays.GetRemoteWorkDaysById;
using HRsystem.Api.Features.RemoteWorkDays;
using HRsystem.Api.Features.Scheduling.RemoteWorkdays.CreateRemoteWorkday;
using HRsystem.Api.Features.Scheduling.RemoteWorkdays.UpdateRemoteWorkDays;
using MediatR;

namespace HRsystem.Api.Features.Scheduling.RemoteWorkdays
{
    public static class RemoteWorkDaysEndpoints
    {
        public static void MapRemoteWorkDaysEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/remoteworkdays").WithTags("RemoteWorkDays");

            // Get all
            group.MapGet("/GetListOfRemoteWorkdays", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllRemoteWorkDaysQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateRemoteWorkdays", async (ISender mediator, CreateRemoteWorkDaysCommand command) =>
            {
                var id = await mediator.Send(command);
                return Results.Ok(new { Success = true, Id = id });
            });

            // Update
            group.MapPut("/UpdateOneOfRemoteWorkdays/{id}", async (ISender mediator, int id, UpdateRemoteWorkDaysCommand command) =>
            {
                if (id != command.Id) return Results.BadRequest();
                var success = await mediator.Send(command);
                return success ? Results.Ok(new { Success = true }) : Results.NotFound();
            });

            // Delete
            group.MapDelete("/DeleteOneOfRemoteWorkdays/{id}", async (ISender mediator, int id) =>
            {
                var success = await mediator.Send(new DeleteRemoteWorkDaysCommand(id));
                return success ? Results.Ok(new { Success = true }) : Results.NotFound();
            });

            group.MapGet("/GetOneRemoteWorkDays/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetRemoteWorkDaysByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"RemoteWorkDays {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

        }
    }
}
