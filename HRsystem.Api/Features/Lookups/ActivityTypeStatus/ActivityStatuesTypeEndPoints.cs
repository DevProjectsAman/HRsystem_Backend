//using HRsystem.Api.Features.ActivityTypeStatus.GetActivityTypeStatus;
//using MediatR;


//namespace HRsystem.Api.Features.ActivityTypeStatus
//{
//    public static class ActivityTypeStatusEndpoint
//    {
//        public static void MapActivityTypeStatusEndpoints(this IEndpointRouteBuilder app)
//        {
//            var group = app.MapGroup("/api/activity-type-status").WithTags("ActivityTypeStatus");

//            // ✅ Create
//            group.MapPost("/", async (CreateActivityTypeStatusCommand command, ISender mediator) =>
//            {
//                var id = await mediator.Send(command);
//                return Results.Ok(new { Success = true, Message = "Created Successfully", Id = id });
//            });

//            // ✅ GetAll
//            group.MapGet("/", async (ISender mediator) =>
//            {
//                var list = await mediator.Send(new GetAllActivityTypeStatusQuery());
//                return Results.Ok(new { Success = true, Data = list });
//            });

//            // ✅ GetById
//            group.MapGet("/{id}", async (int id, ISender mediator) =>
//            {
//                var item = await mediator.Send(new GetActivityTypeStatusByIdQuery(id));
//                if (item == null) return Results.NotFound(new { Success = false, Message = "Not Found" });
//                return Results.Ok(new { Success = true, Data = item });
//            });

//            // ✅ Update
//            group.MapPut("/{id}", async (int id, UpdateActivityTypeStatusCommand command, ISender mediator) =>
//            {
//                if (id != command.ActivityTypeStatusId) return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

//                var result = await mediator.Send(command);
//                if (result == null) return Results.NotFound(new { Success = false, Message = "Not Found" });
//                return Results.Ok(new { Success = true, Message = "Updated Successfully" });
//            });

//            // ✅ Delete
//            group.MapDelete("/{id}", async (int id, ISender mediator) =>
//            {
//                var result = await mediator.Send(new DeleteActivityTypeStatusCommand(id));
//                if (result == null) return Results.NotFound(new { Success = false, Message = "Not Found" });
//                return Results.Ok(new { Success = true, Message = "Deleted Successfully" });
//            });
//        }
//    }
//}