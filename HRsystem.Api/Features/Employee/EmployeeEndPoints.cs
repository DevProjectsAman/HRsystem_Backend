
//using HRsystem.Api.Features.Employee.DTO;
//using MediatR;

//namespace HRsystem.Api.Features.Employee

//{


//    public static class EmployeeEndpoints
//    {
//        public static void MapEmployeeEndpoints(this IEndpointRouteBuilder app)
//        {
//            var group = app.MapGroup("/api/Employees").WithTags("Employee Management");

//            group.MapPost("AddNewEmployee/", async (EmployeeCreateDto dto, IMediator mediator) =>
//                Results.Ok(await mediator.Send(new CreateEmployeeCommand(dto))));

//            group.MapPut("UpdateEmployee/{id:int}", async (int id, EmployeeUpdateDto dto, IMediator mediator) =>
//            {
//                if (id != dto.EmployeeId) return Results.BadRequest("ID mismatch");
//                return Results.Ok(await mediator.Send(new UpdateEmployeeCommand(dto)));
//            });

//            group.MapPut("completeEmployeeProfile/{id:int}", async (int id, CompleteEmployeeProfileDto dto, IMediator mediator) =>
//            {
//                // هنا مفيش داعي نتحقق من الـ id في الـ dto لأنه أصلاً مش موجود
//                var result = await mediator.Send(new CompleteEmployeeProfile.CompleteEmployeeProfileCommand(id, dto));

//                return result ? Results.Ok("Employee profile updated successfully")
//                              : Results.BadRequest("Failed to update profile");
//            });

//            group.MapPut("employeeWorkSchedule/{id:int}", async (int id, EmployeeWorkScheduleDto dto, IMediator mediator) =>
//            {
//                var updatedEmployee = await mediator.Send(
//                    new EmployeeWorkSchedule.EmployeeWorkScheduleCommand(id, dto)
//                );

//                if (updatedEmployee == null)
//                    return Results.BadRequest("Failed to update employee work schedule");

//                return Results.Ok(updatedEmployee); // ✅ بيرجع الـ object بالكامل كـ JSON
//            });
//            group.MapGet("GetEmployee/{id:int}", async (int id, IMediator mediator) =>
//            {
//                var employee = await mediator.Send(new GetEmployeeByIdQuery(id));
//                return employee is null ? Results.NotFound() : Results.Ok(employee);
//            });


//        }
//    }

//}


using HRsystem.Api.Features.Employee.Commands;
using HRsystem.Api.Features.Employee.DTO;
//using HRsystem.Api.Features.Employee.UpdateEmployee;
using HRsystem.Api.Features.Employee.Commands;
using MediatR;

namespace HRsystem.Api.Features.Employee
{
    public static class EmployeeEndpoints
    {
        public static void MapEmployeeEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/employees").WithTags("Employees");

            // ✅ Get All
            group.MapGet("/GetListOFEmployees", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllEmployeesQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // ✅ Get By Id
            group.MapGet("/GetOneOFEmployee/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetEmployeeByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = "Employee not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });



           
            // ✅ Get Employee By HR Code
            group.MapGet("/GetEmployeeByCodeHr/{employeeCodeHr}", async (string employeeCodeHr, ISender mediator) =>
            {
                var result = await mediator.Send(new GetEmployeeByCodeHrQuery(employeeCodeHr));

                if (result == null || result.Data == null)
                    return Results.NotFound(new { Success = false, Message = "Employee not found" });

                return Results.Ok(new { Success = true, Data = result.Data });
            })
            .WithName("GetEmployeeByCodeHr")
            .WithSummary("Get employee details by HR Code")
            .WithDescription("Fetch employee details using HR Code.");


            // ✅ Create
            group.MapPost("/CreateEmployee", async (EmployeeCreateDto dto, ISender mediator) =>
            {
                var result = await mediator.Send(new CreateEmployeeCommand(dto));
                return Results.Created($"/api/employees/{result.EmployeeId}", new { Success = true, Data = result });
            });

            // ✅ Update
            //group.MapPut("/UpdateEmployee/{id}", async (int id, EmployeeUpdateDto dto, ISender mediator) =>
            //{
            //    if (id != dto.EmployeeId)
            //        return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

            //    var cmd = new UpdateEmployeeCommand(id, dto); // لازم id + dto
            //    var result = await mediator.Send(cmd);

            //    return result == null
            //        ? Results.NotFound(new { Success = false, Message = "Employee not found" })
            //        : Results.Ok(new { Success = true, Data = result });
            //});

            group.MapPut("/UpdateEmployee/{id}", async (int id, EmployeeUpdateDto dto, ISender mediator) =>
            {
                if (id != dto.EmployeeId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var cmd = new UpdateEmployeeCommand(dto); // ✅ كده تمام لأنه بياخد dto بس
                var result = await mediator.Send(cmd);

                return result == null
                   ? Results.NotFound(new { Success = false, Message = "Employee not found" })
                   : Results.Ok(new { Success = true, Data = result });
            });


            // ✅ Delete
            group.MapDelete("/DeleteEmployee/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteEmployeeCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = "Employee not found" })
                    : Results.Ok(new { Success = true, Message = "Deleted successfully" });
            });
        }
    }
}
