using FluentValidation;
using HRsystem.Api.Features.Organization.Department.CreateDepartment;
using HRsystem.Api.Features.Organization.Department.DeleteDepartment;
using HRsystem.Api.Features.Organization.Department.GetAllDeparmentsLocalized;
using HRsystem.Api.Features.Organization.Department.GetAllDepartments;
using HRsystem.Api.Features.Organization.Department.GetDepartmentById;
using HRsystem.Api.Features.Organization.Department.UpdateDepartment;
using HRsystem.Api.Shared.DTO;
using MediatR;

namespace HRsystem.Api.Features.Organization.Department
{
    public static class DepartmentEndpoints
    {
        public static void MapDepartmentEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Organization/departments")
                           .WithTags("Departments");

            // ✅ Get All (localized)
            group.MapGet("/listOfDepartmentsLocalized", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllDepartmentsQuery());
                return Results.Ok(new ResponseResultDTO<object>
                {
                    Success = true,
                    Message = "Departments retrieved successfully",
                    Data = result
                });
            });

            // ✅ Get All (non-localized)
            group.MapGet("/listOfDepartments/{CompanyId}", async (int CompanyId, ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllDepartmentsLocalized(CompanyId));
                return Results.Ok(new ResponseResultDTO<object>
                {
                    Success = true,
                    Message = "Departments retrieved successfully",
                    Data = result
                });
            });

            // ✅ Get One
            group.MapGet("/GetOneDepartment/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetDepartmentByIdQuery(id));

                if (result == null)
                {
                    return Results.NotFound(new ResponseResultDTO
                    {
                        Success = false,
                        Message = $"Department {id} not found"
                    });
                }

                return Results.Ok(new ResponseResultDTO<object>
                {
                    Success = true,
                    Message = "Department retrieved successfully",
                    Data = result
                });
            });

            // ✅ Create
            group.MapPost("/CreateDepartment", async (
                CreateDepartmentCommand cmd,
                ISender mediator,
                IValidator<CreateDepartmentCommand> validator) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);

                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = validationResult.Errors.Select(e => new ResponseErrorDTO
                        {
                            Property = e.PropertyName,
                            Error = e.ErrorMessage
                        }).ToList()
                    });
                }

                var result = await mediator.Send(cmd);

                return Results.Created(
                    $"/api/Organization/departments/{result.DepartmentId}",
                    new ResponseResultDTO<object>
                    {
                        Success = true,
                        Message = "Department created successfully",
                        Data = result
                    });
            });

            // ✅ Update
            group.MapPut("/UpdateDepartment", async (
                
                UpdateDepartmentCommand cmd,
                ISender mediator,
                IValidator<UpdateDepartmentCommand> validator) =>
            {
                
                var validationResult = await validator.ValidateAsync(cmd);

                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = validationResult.Errors.Select(e => new ResponseErrorDTO
                        {
                            Property = e.PropertyName,
                            Error = e.ErrorMessage
                        }).ToList()
                    });
                }

                var result = await mediator.Send(cmd);

                if (result == null)
                {
                    return Results.NotFound(new ResponseResultDTO
                    {
                        Success = false,
                        Message = $"Department {cmd.DepartmentId} not found"
                    });
                }

                return Results.Ok(new ResponseResultDTO<object>
                {
                    Success = true,
                    Message = "Department updated successfully",
                    Data = result
                });
            });

            // ✅ Delete
            group.MapDelete("/DeleteDepartment/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteDepartmentCommand(id));

                if (!result)
                {
                    return Results.NotFound(new ResponseResultDTO
                    {
                        Success = false,
                        Message = $"Department {id} not found"
                    });
                }

                return Results.Ok(new ResponseResultDTO
                {
                    Success = true,
                    Message = $"Department {id} deleted successfully"
                });
            });
        }
    }
}
