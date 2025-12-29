//using HRsystem.Api.Features.EmployeeDashboard.GetAllActivities;
//using HRsystem.Api.Features.EmployeeDashboard.GetApprovedActivites;
//using HRsystem.Api.Features.EmployeeDashboard.GetRejectedActivities;
//using MediatR;

//namespace HRsystem.Api.Features.EmployeeDashboard.GetPendingActivities
//{
//    public static class GetPendingActivitiesEndPoints
//    {
//        public static void MapPendingActivitiesEndPoints(this IEndpointRouteBuilder app)
//        {
//            var group = app.MapGroup("/api/employee-dashboard").WithTags("Employee Dashboard");

//            // Get pending activities for current user
//            group.MapGet("/pending", async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new GetPendingActivitiesQuery());
//                if (result == null || !result.Any())
//                    return Results.NotFound(new { Success = false, Message = "No pending activities found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });

//            group.MapGet("/count of all activites", async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new GetActivitiesStatusCountQuery());
//                if (result == null )
//                    return Results.NotFound(new { Success = false, Message = "No  activities found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });

//            group.MapGet("/Approved", async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new GetApprovedActivitiesQueury());
//                if (result == null || !result.Any())
//                    return Results.NotFound(new { Success = false, Message = "No Approved activities found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });

//            group.MapGet("/Rejected", async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new GetRejectedActivitiesQueury());
//                if (result == null || !result.Any())
//                    return Results.NotFound(new { Success = false, Message = "No Rejected activities found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });

//            group.MapGet("/AllActivities", async (ISender mediator) =>
//            {
//                var result = await mediator.Send(new GetAllActivitiesQuery());
//                if (result == null || !result.Any())
//                    return Results.NotFound(new { Success = false, Message = "No  activities found" });

//                return Results.Ok(new { Success = true, Data = result });
//            });


//        }
//    }
//}
using HRsystem.Api.Features.EmployeeDashboard.GetAllActivities;
using HRsystem.Api.Features.EmployeeDashboard.GetApprovedActivites;
using HRsystem.Api.Features.EmployeeDashboard.GetRejectedActivities;
using MediatR;
using HRsystem.Api.Shared.DTO;
using Microsoft.AspNetCore.Authorization;

namespace HRsystem.Api.Features.EmployeeDashboard.GetPendingActivities
{
    public static class GetPendingActivitiesEndPoints
    {
        public static void MapPendingActivitiesEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/employee-dashboard")
                           .WithTags("Employee Dashboard");

            // ============================================
            // Helper: Build Standard Response
            // ============================================
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

            // ============================================
            // Helper: Error Response
            // ============================================
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

            // ============================================
            // Pending activities
            // ============================================
            group.MapGet("/pending", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetPendingActivitiesQuery());
                    return BuildResponse(result,
                        "No pending activities found",
                        "Pending activities loaded successfully");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ============================================
            // Count of all activities
            // ============================================
            group.MapGet("/count-of-all-activities", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetActivitiesStatusCountQuery());
                    return BuildResponse(result,
                        "No activities found",
                        "Activities count loaded");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ============================================
            // Approved activities
            // ============================================
            group.MapGet("/approved", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetApprovedActivitiesQueury());
                    return BuildResponse(result,
                        "No approved activities found",
                        "Approved activities loaded");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ============================================
            // Rejected activities
            // ============================================
            group.MapGet("/rejected", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetRejectedActivitiesQueury());
                    return BuildResponse(result,
                        "No rejected activities found",
                        "Rejected activities loaded");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });

            // ============================================
            // All activities
            // ============================================
            group.MapGet("/all-activities", [Authorize] async (ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new GetAllActivitiesQuery());
                    return BuildResponse(result,
                        "No activities found",
                        "All activities loaded");
                }
                catch (Exception ex)
                {
                    return BuildError(ex);
                }
            });
        }
    }
}

