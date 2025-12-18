//using HRsystem.Api.Features.EmployeeDashboard.ManagerActivity;
//using MediatR;

//namespace HRsystem.Api.Features.EmployeeDashboard.mangeractivity
//{
//    public static class GetPendingStatuesForManager
//    {
//        public static void MapPendingStatuesForManager(this IEndpointRouteBuilder app)
//        {
//           // var group = app.MapGroup("/api/activities").WithTags("Activities");
//            var group = app.MapGroup("/api/employee-dashboard").WithTags("Employee Dashboard");

//            // Get subordinates with pending activities
//            group.MapGet("/subordinates/pending", async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new GetSubordinatesPendingActivitiesQuery());
//                if (result == null || !result.Any())
//                    return Results.NotFound(new { Success = false, Message = "No subordinates or pending activities found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });

//            group.MapGet("/subordinates/Numberofpending", async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new GetNumberOfPendingReqForManager());
//                if (result == null)
//                    return Results.NotFound(new { Success = false, Message = "No subordinates or pending activities found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });


//            group.MapGet("/subordinates/IsManager", async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new CheckManagerQuery());
//                if (result == null)
//                    return Results.NotFound(new { Success = false, Message = "Employee Isn't A Manager" });

//                return Results.Ok(new { Success = true, Data = result });
//            });

//        }
//    }
//}using MediatR;
using HRsystem.Api.Features.EmployeeDashboard.EmployeeApp;
using HRsystem.Api.Features.EmployeeDashboard.GetAllActivities;
using HRsystem.Api.Features.EmployeeDashboard.GetApprovedActivites;
using HRsystem.Api.Features.EmployeeDashboard.GetPendingActivities;
using HRsystem.Api.Features.EmployeeDashboard.GetRejectedActivities;
using HRsystem.Api.Features.EmployeeDashboard.mangeractivity;
using HRsystem.Api.Shared.DTO;
using MediatR;

namespace HRsystem.Api.Features.EmployeeDashboard.ManagerActivity
{
    public static class ManagerActivitiesEndpoints
    {
        public static void MapPendingStatuesForManager(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/employee-dashboard")
                           .WithTags("Employee Dashboard");

            // ===========================================================
            // Helper: Build OK Response
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

            // ===========================================================
            // Helper: Build Error Response
            // ===========================================================
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
            // (1) Get Pending Activities for Manager
            // ===========================================================
            group.MapGet("/subordinates/pending", async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetSubordinatesPendingActivitiesQuery());
                    return BuildResponse(result,
                        "No pending activities found for your subordinates",
                        "Pending activities loaded successfully");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ===========================================================
            // (2) Get Number of Pending Requests for Manager
            // ===========================================================
            group.MapGet("/subordinates/number-of-activities-types", async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetNumberOfPendingReqForManager());
                    return BuildResponse(result,
                        "No activities requests found",
                        "activities requests count loaded successfully");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ===========================================================
            // (3) Check if employee is manager
            // ===========================================================
            group.MapGet("/subordinates/is-manager", async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new CheckManagerQuery());
                    return BuildResponse(result,
                        "Employee is not a manager",
                        "Manager status loaded");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            group.MapPost("/change-status", async (ISender mediator, ChangeActivityRequestStatuesDto dto) =>
            {
                try
                {
                    var result = await mediator.Send(new ChangeActivityRequestStatues(dto));

                    return result
                        ? BuildResponse(true,
                            "Activity not found",
                            "Activity status updated successfully")
                        : BuildResponse(false,
                            "Activity not found",
                            "Activity status Not updated ");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });
            // ============================================
            // Pending activities
            // ============================================
            group.MapGet("/Manager/pending", async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetMangerPendingActivitiesQuery());
                    return BuildResponse(result,
                        "No pending activities found",
                        "Pending activities loaded successfully");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });
            group.MapGet("/Manager/Rejected", async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetMangerRejectedActivitiesQueury ());
                    return BuildResponse(result,
                        "No Rejected activities found",
                        "Rejected activities loaded successfully");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });
            group.MapGet("/Manager/Approved", async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetMangerApprovedActivitiesQueury());
                    return BuildResponse(result,
                        "No Approved activities found",
                        "Approved activities loaded successfully");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });
            group.MapGet("/Manager/All-Activites", async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetMangerAllActivitiesQuery());
                    return BuildResponse(result,
                        "No  activities found",
                        " activities loaded successfully");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });
        }
    }
}
