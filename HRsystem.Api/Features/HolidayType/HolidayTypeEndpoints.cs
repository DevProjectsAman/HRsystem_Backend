using HRsystem.Api.Features.HolidayType.CreateHolidayType;
using HRsystem.Api.Features.HolidayType.DeleteHolidayType;
using HRsystem.Api.Features.HolidayType.GetAllHolidayTypes;
using HRsystem.Api.Features.HolidayType.GetHolidayTypeById;
using HRsystem.Api.Features.HolidayType.UpdateHolidayType;
using HRsystem.Api.Shared.DTO;
using MediatR;

namespace HRsystem.Api.Features.HolidayType
{
    public static class HolidayTypeEndpoints
    {
        public static void MapHolidayTypeEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/holidaytypes").WithTags("Holiday Types");

            group.MapGet("/GetListOfHolidayTypes", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllHolidayTypesQuery());
                return Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });

            group.MapGet("/GetOneOfHolidayTypes/{id:int}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetHolidayTypeByIdQuery(id));
                if (result == null) return Results.NotFound(new ResponseResultDTO { Success = false, Message = "Not found" });
                return Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            });

            group.MapPost("/", async (CreateHolidayTypeDto dto, ISender mediator) =>
            {
                var id = await mediator.Send(new CreateHolidayTypeCommand(dto));
                return Results.Ok(new ResponseResultDTO<int> { Success = true, Data = id });
            });

            group.MapPut("/{id:int}", async (int id, UpdateHolidayTypeDto dto, ISender mediator) =>
            {
                dto.HolidayTypeId = id;
                var updated = await mediator.Send(new UpdateHolidayTypeCommand(dto));
                return updated
                    ? Results.Ok(new ResponseResultDTO { Success = true })
                    : Results.NotFound(new ResponseResultDTO { Success = false, Message = "Not found" });
            });

            group.MapDelete("/{id:int}", async (int id, ISender mediator) =>
            {
                var deleted = await mediator.Send(new DeleteHolidayTypeCommand(id));
                return deleted
                    ? Results.Ok(new ResponseResultDTO { Success = true })
                    : Results.NotFound(new ResponseResultDTO { Success = false, Message = "Not found" });
            });
        }
    }
}
