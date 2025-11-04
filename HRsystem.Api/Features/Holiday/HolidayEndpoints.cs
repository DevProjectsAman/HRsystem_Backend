using HRsystem.Api.Features.Holiday.CreateHoliday;
using HRsystem.Api.Features.Holiday.DeleteHoliday;
using HRsystem.Api.Features.Holiday.GetAllHolidays;
using HRsystem.Api.Features.Holiday.GetHolidayById;
using HRsystem.Api.Features.Holiday.UpdateHoliday;
using HRsystem.Api.Shared.DTO;
using MediatR;

namespace HRsystem.Api.Features.Holiday
{
    public static class HolidayEndpoints
    {
        public static void MapHolidayEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/holidays").WithTags("Holidays");

            group.MapGet("/GetAllHolidays/{companyId}", async (ISender mediator,int companyId) =>
            {
                var result = await mediator.Send(new GetAllHolidaysQuery(companyId));
                return Results.Ok(new ResponseResultDTO<object>
                {
                    Success = true,
                    Data = result
                });
            });

            group.MapGet("/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetHolidayByIdQuery(id));
                return result == null
                    ? Results.NotFound(new ResponseResultDTO { Success = false, Message = "Holiday not found" })
                    : Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });

            group.MapPost("/CreateHoliday", async (CreateHolidayCommand cmd, ISender mediator) =>
            {
                var result = await mediator.Send(cmd);
                return Results.Created($"/api/holidays/{result?.HolidayId}", new ResponseResultDTO<object>
                {
                    Success = true,
                    Message = "Created",
                    Data = result
                });
            });

            group.MapPut("/{id}", async (int id, UpdateHolidayCommand cmd, ISender mediator) =>
            {
                if (id != cmd.HolidayId)
                    return Results.BadRequest(new ResponseResultDTO { Success = false, Message = "Id mismatch" });

                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new ResponseResultDTO { Success = false, Message = "Holiday not found" })
                    : Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });

            group.MapDelete("/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteHolidayCommand(id));
                return !result
                    ? Results.NotFound(new ResponseResultDTO { Success = false, Message = "Holiday not found" })
                    : Results.Ok(new ResponseResultDTO { Success = true, Message = "Deleted successfully" });
            });
        }
    }

}
