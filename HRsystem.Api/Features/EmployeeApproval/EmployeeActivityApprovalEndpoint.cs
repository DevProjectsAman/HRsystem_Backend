using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HRsystem.Api.Features.EmployeeApproval
{
    public static class EmployeeActivityApprovalEndpoint
    {
        public static void MapEmployeeActivityApprovalEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/employee-activities/Approval").WithTags("Employee Activities");

            // Approve/Reject Activity
            group.MapPost("/approval/{activityId}", [Authorize] async (
                long activityId,
                ApproveEmployeeActivityCommand command,
                ISender mediator) =>
            {
                if (activityId != command.ActivityId)
                    return Results.BadRequest("ActivityId mismatch");

                var result = await mediator.Send(command);
                return Results.Ok(new { Success = true, Message = "Activity status updated successfully", Data = result });
            });
        }

    }
}
