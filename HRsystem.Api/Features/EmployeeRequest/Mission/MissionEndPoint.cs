using HRsystem.Api.Features.Mission.CreateMission;
using HRsystem.Api.Features.Project.CreateProject;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HRsystem.Api.Features.Mission
{

    public static class MissionEndPoint
    {
        public static void MapMissionEndPoint(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/missionRequest").WithTags("requests");

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
