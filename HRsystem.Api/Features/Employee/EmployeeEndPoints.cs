
using HRsystem.Api.Features.Employee.DTO;
using MediatR;

namespace HRsystem.Api.Features.Employee

{


    public static class EmployeeEndpoints
    {
        public static void MapEmployeeEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/ManageEmployees").WithTags("Employee Management");

            group.MapPost("AddNewEmployee/", async (EmployeeCreateDto dto, IMediator mediator) =>
                Results.Ok(await mediator.Send(new CreateEmployeeCommand(dto))));

            group.MapPut("UpdateEmployee/{id:int}", async (int id, EmployeeUpdateDto dto, IMediator mediator) =>
            {
                if (id != dto.EmployeeId) return Results.BadRequest("ID mismatch");
                return Results.Ok(await mediator.Send(new UpdateEmployeeCommand(dto)));
            });

            group.MapPut("completeEmployeeProfile/{id:int}", async (int id, CompleteEmployeeProfileDto dto, IMediator mediator) =>
            {
                // هنا مفيش داعي نتحقق من الـ id في الـ dto لأنه أصلاً مش موجود
                var result = await mediator.Send(new CompleteEmployeeProfile.CompleteEmployeeProfileCommand(id, dto));

                return result ? Results.Ok("Employee profile updated successfully")
                              : Results.BadRequest("Failed to update profile");
            });

            group.MapGet("GetEmployee/{id:int}", async (int id, IMediator mediator) =>
            {
                var employee = await mediator.Send(new GetEmployeeByIdQuery(id));
                return employee is null ? Results.NotFound() : Results.Ok(employee);
            });

            
        }
    }

}
