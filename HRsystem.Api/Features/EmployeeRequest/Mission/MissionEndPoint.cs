using HRsystem.Api.Features.EmployeeRequest.Mission.CreateMission;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HRsystem.Api.Features.EmployeeRequest.Mission
{

    public static class MissionEndPoint
    {
        public static void MapMissionEndPoint(this IEndpointRouteBuilder app)
        {
            //var group = app.MapGroup("/api/missionRequest").WithTags("requests");

            var group = app.MapGroup("/api/employee-requests").WithTags("Employee Requests");

            // ✅ Use CreateMissionCommand instead of CreateProjectCommand
            group.MapPost("/CreateMission", [Authorize]async (CreateMissionCommand command, ISender mediator) =>
            {
                var result = await mediator.Send(command);

                //return Results.Created($"/api/missionRequest/{result.MissionId}", new
                //{
                //    Success = true,
                //    Data = result
                //});

                return Results.Ok(new
                {
                    Success = true,
                    Message = "mission requested successfully",
                    Data = result
                });
            });
        }
    }
 } 
