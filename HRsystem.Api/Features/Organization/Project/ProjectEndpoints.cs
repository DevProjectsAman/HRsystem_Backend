using HRsystem.Api.Features.Organization.Project.CreateProject;
using HRsystem.Api.Features.Organization.Project.GetAllProjects;
using HRsystem.Api.Features.Organization.Project.GetProjectById;
using HRsystem.Api.Features.Organization.Project.UpdateProject;
using HRsystem.Api.Features.Project.DeleteProject;
using MediatR;

namespace HRsystem.Api.Features.Organization.Project
{
    public static class ProjectEndpoints
    {
        public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Organization/Project").WithTags("Projects");

            // Get all
            group.MapGet("/ListOfProjects/{CompanyId}", async (ISender mediator,int CompanyId) =>
            {
                var result = await mediator.Send(new GetAllProjectsCommand(CompanyId));
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            group.MapGet("/GetOneProject/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetProjectByIdCommand(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Project {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateProject", async (CreateProjectCommand command, ISender mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/projects/{result.ProjectId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/UpdateProject/{id}", async (int id, UpdateProjectCommand command, ISender mediator) =>
            {
                if (id != command.ProjectId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var result = await mediator.Send(command);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Project {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/DeleteProject/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteProjectCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Project {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Project {id} deleted successfully" });
            });
        }
    }
}
