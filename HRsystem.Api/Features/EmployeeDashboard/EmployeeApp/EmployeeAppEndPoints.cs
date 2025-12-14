//using MediatR;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
//{
//    public static class EmployeeAppEndPoints
//    {
//        public static void MapEmployeeAppEndPoints(this IEndpointRouteBuilder app)
//        {
//            var group = app.MapGroup("/api/employee-App").WithTags("Employee App");

//            // Get pending activities for current user
//            group.MapGet("/EmployeeInfo", [Authorize] async ( ISender mediator) =>
//            {
//                var result = await mediator.Send(new EmployeeInfoQueury());
//                if (result == null)
//                    return Results.NotFound(new { Success = false, Message = "No Employee  found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });

//            group.MapGet("/AnnualBalance", [Authorize] async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new EmployeeAnnualBalance());
//                if (result == null)
//                    return Results.NotFound(new { Success = false, Message = "No Employee  found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });

//            group.MapGet("/CasualBalance", [Authorize] async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new EmployeeCasualBalance());
//                if (result == null)
//                    return Results.NotFound(new { Success = false, Message = "No Employee  found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });

//            //  Get Employee Days History by Date Filter
//            group.MapPost("/EmployeeDaysHistory", [Authorize] async (ISender mediator, [FromBody] GetEmployeeDaysHistoryByFilterQueury query) =>
//            {
//                var result = await mediator.Send(query);

//                if (result == null || !result.Any())
//                    return Results.NotFound(new { Success = false, Message = "No records found for this date range." });

//                return Results.Ok(new { Success = true, Data = result });
//            });

//            group.MapGet("/FullDashboard", [Authorize] async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new EmployeeFullDashboardQuery());
//                if (result == null)
//                    return Results.NotFound(new { Success = false, Message = "No Employee data found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });

//            group.MapGet("/GetEmployeeCheckInLog", [Authorize] async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new GetEmployeeCheckInLogQueury());
//                if (result == null)
//                    return Results.NotFound(new { Success = false, Message = "No Employee data found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });
//            group.MapGet("/GetEmployeeCheckOutLog", [Authorize] async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new GetEmployeeCheckOutLogQueury());
//                if (result == null)
//                    return Results.NotFound(new { Success = false, Message = "No Employee data found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });
//            group.MapGet("/GetEmployeeTotalWorkingHours", [Authorize] async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new GetEmployeeTotalWorkingHoursQueury());
//                if (result == null)
//                    return Results.NotFound(new { Success = false, Message = "No Employee data found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });
//            group.MapGet("/EmployeeGetShift", [Authorize] async (ISender mediator, [FromQuery] TimeOnly time) =>
//            {
//                var result = await mediator.Send(new EmployeeGetShiftQueury(time));
//                if (result == null)
//                    return Results.NotFound(new { Success = false, Message = "No Employee data found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });
//            group.MapGet("/EmployeeRequests", [Authorize] async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new EmployeeRequestsDashboardQuery());
//                if (result == null)
//                    return Results.NotFound(new { Success = false, Message = "No Employee data found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });


//        }
//    }
//}
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HRsystem.Api.Shared.DTO;

namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
    public static class EmployeeAppEndPoints
    {
        public static void MapEmployeeAppEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/employee-app")
                           .WithTags("Employee App");

            // ===========================================================
            // Helpers
            // ===========================================================
            static IResult BuildResponse<T>(T? data, string emptyMessage, string successMessage)
            {
                bool hasData = data is not null &&
                               (!(data is System.Collections.ICollection c) || c.Count > 0);

                return Results.Ok(new ResponseResultDTO<T?>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = hasData ? successMessage : emptyMessage,
                    Data = hasData ? data : default
                });
            }

            static IResult BuildError(Exception ex)
            {
                return Results.Ok(new ResponseResultDTO
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Unexpected error occurred",
                    Errors = new List<ResponseErrorDTO>
                    {
                        new ResponseErrorDTO
                        {
                            Property = "",
                            Error = ex.Message
                        }
                    }
                });
            }

            // ===========================================================
            // Employee Info
            // ===========================================================
            group.MapGet("/employee-info", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new EmployeeInfoQueury());
                    return BuildResponse(result,
                        "No employee data found",
                        "Employee info loaded successfully");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ===========================================================
            // Annual Balance
            // ===========================================================
            group.MapGet("/annual-balance", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new EmployeeAnnualBalance());
                    return BuildResponse(result,
                        "No annual balance data found",
                        "Annual balance loaded successfully");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ===========================================================
            // Casual Balance
            // ===========================================================
            group.MapGet("/casual-balance", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new EmployeeCasualBalance());
                    return BuildResponse(result,
                        "No casual balance data found",
                        "Casual balance loaded successfully");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ===========================================================
            // Employee Days History (Filter)
            // ===========================================================
            group.MapPost("/employee-days-history", [Authorize]
            async (ISender mediator, [FromBody] GetEmployeeDaysHistoryByFilterQueury query) =>
            {
                try
                {
                    var result = await mediator.Send(query);
                    return BuildResponse(result,
                        "No records found for this date range",
                        "Employee days history loaded");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ===========================================================
            // Full Dashboard
            // ===========================================================
            group.MapGet("/full-dashboard", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new EmployeeFullDashboardQuery());
                    return BuildResponse(result,
                        "No dashboard data found",
                        "Employee dashboard loaded");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ===========================================================
            // Check-in Log
            // ===========================================================
            group.MapGet("/check-in-log", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetEmployeeCheckInLogQueury());
                    return BuildResponse(result,
                        "No check-in log found",
                        "Check-in log loaded");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ===========================================================
            // Check-out Log
            // ===========================================================
            group.MapGet("/check-out-log", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetEmployeeCheckOutLogQueury());
                    return BuildResponse(result,
                        "No check-out log found",
                        "Check-out log loaded");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ===========================================================
            // Total Working Hours
            // ===========================================================
            group.MapGet("/total-working-hours", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetEmployeeTotalWorkingHoursQueury());
                    return BuildResponse(result,
                        "No working hours data found",
                        "Total working hours loaded");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ===========================================================
            // Get Shift
            // ===========================================================
            group.MapGet("/shift", [Authorize] async (ISender mediator, [FromQuery] TimeOnly time) =>
            {
                try
                {
                    var result = await mediator.Send(new EmployeeGetShiftQueury(time));
                    return BuildResponse(result,
                        "No shift found",
                        "Shift loaded");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ===========================================================
            // Employee Requests Dashboard
            // ===========================================================
            group.MapGet("/EmployeeRequests", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new EmployeeRequestsDashboardQuery());
                    return BuildResponse(result,
                        "No requests found",
                        "Employee requests loaded");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });
            group.MapGet("/EmployeeProfile", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new EmployeeProfileQueury());
                    return BuildResponse(result,
                        "No Profile found",
                        "Employee Profile loaded");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });
            
        }
    }
}