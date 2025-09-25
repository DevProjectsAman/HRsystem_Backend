using FluentValidation;
using HRsystem.Api.Features.Organization.Department.CreateDepartment;
using HRsystem.Api.Features.Organization.Department.DeleteDepartment;
using HRsystem.Api.Features.Organization.Department.GetAllDepartments;
using HRsystem.Api.Features.Organization.Department.GetDepartmentById;
using HRsystem.Api.Features.Organization.Department.UpdateDepartment;
using MediatR;

namespace HRsystem.Api.Features.Organization.Department
{
    public static class DepartmentEndpoints
    {
        public static void MapDepartmentEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Organization/departments").WithTags("Departments");

            // Get All
            group.MapGet("/listOfDepartments", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllDepartmentsQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get One
            group.MapGet("/GetOneDepartment/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetDepartmentByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Department {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateDepartment", async (CreateDepartmentCommand cmd, ISender mediator, IValidator<CreateDepartmentCommand> validator) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new
                    {
                        Success = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage)
                    });

                var result = await mediator.Send(cmd);
                return Results.Created($"/api/departments/{result.DepartmentId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/UpdateDepartment/{id}", async (int id, UpdateDepartmentCommand cmd, ISender mediator, IValidator<UpdateDepartmentCommand> validator) =>
            {
                if (id != cmd.DepartmentId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new
                    {
                        Success = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage)
                    });

                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Department {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/DeleteDepartment/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteDepartmentCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Department {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Department {id} deleted successfully" });
            });
        }
    }
}
