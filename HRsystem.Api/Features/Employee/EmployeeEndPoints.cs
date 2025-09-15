
using HRsystem.Api.Features.Employee.DTO;
using MediatR;

namespace HRsystem.Api.Features.Employee

{


    public static class EmployeeEndpoints
    {
        public static void MapEmployeeEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/ManageEmployees").WithTags("Manage Employees");

            group.MapPost("AddNewEmployee/", async (EmployeeCreateDto dto, IMediator mediator) =>
                Results.Ok(await mediator.Send(new CreateEmployeeCommand(dto))));

            group.MapPut("UpdateEmployee/{id:int}", async (int id, EmployeeUpdateDto dto, IMediator mediator) =>
            {
                if (id != dto.EmployeeId) return Results.BadRequest("ID mismatch");
                return Results.Ok(await mediator.Send(new UpdateEmployeeCommand(dto)));
            });

           
            group.MapGet("GetEmployee/{id:int}", async (int id, IMediator mediator) =>
            {
                var employee = await mediator.Send(new GetEmployeeByIdQuery(id));
                return employee is null ? Results.NotFound() : Results.Ok(employee);
            });

            
        }
    }

}
