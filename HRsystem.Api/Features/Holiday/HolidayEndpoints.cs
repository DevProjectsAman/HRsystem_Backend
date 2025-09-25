using HRsystem.Api.Features.Holiday.CreateHoliday;
using HRsystem.Api.Features.Holiday.DeleteHoliday;
using HRsystem.Api.Features.Holiday.GetAllHolidays;
using HRsystem.Api.Features.Holiday.GetHolidayById;
using HRsystem.Api.Features.Holiday.UpdateHoliday;
using MediatR;

namespace HRsystem.Api.Features.Holiday
{
    public static class HolidayEndpoints
    {
        public static void MapHolidayEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/holidays").WithTags("Holidays");

            group.MapGet("/", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllHolidaysQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            group.MapGet("/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetHolidayByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = "Holiday not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            group.MapPost("/", async (CreateHolidayCommand cmd, ISender mediator) =>
            {
                var result = await mediator.Send(cmd);
                return Results.Created($"/api/holidays/{result.HolidayId}", new { Success = true, Data = result });
            });

            group.MapPut("/{id}", async (int id, UpdateHolidayCommand cmd, ISender mediator) =>
            {
                if (id != cmd.HolidayId) return Results.BadRequest(new { Success = false, Message = "Id mismatch" });
                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = "Holiday not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            group.MapDelete("/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteHolidayCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = "Holiday not found" })
                    : Results.Ok(new { Success = true, Message = "Deleted successfully" });
            });
        }
    }

}
