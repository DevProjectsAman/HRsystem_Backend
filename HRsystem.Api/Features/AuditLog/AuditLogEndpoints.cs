using HRsystem.Api.Features.AuditLog.CreateAuditLog;
using HRsystem.Api.Features.AuditLog.GetAllAuditLogs;
using HRsystem.Api.Features.AuditLog.GetAuditLogById;
using HRsystem.Api.Features.AuditLog.UpdateAuditLog;
using HRsystem.Api.Features.AuditLog.DeleteAuditLog;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HRsystem.Api.Features.AuditLog
{
    public static class AuditLogEndpoints
    {
        public static void MapAuditLogEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/auditlogs").WithTags("AuditLogs");

            // Get all
            group.MapGet("/ListOfAuditLogs", [Authorize] async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllAuditLogsCommand());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            group.MapGet("/GetOneAuditLog/{id}", [Authorize] async (long id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetAuditLogByIdCommand(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Audit log {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateAuditLog", [Authorize] async (CreateAuditLogCommand command, ISender mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/auditlogs/{result.AuditId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/UpdateAuditLog/{id}", [Authorize] async (long id, UpdateAuditLogCommand command, ISender mediator) =>
            {
                if (id != command.AuditId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var result = await mediator.Send(command);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Audit log {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/DeleteAuditLog/{id}", [Authorize] async (long id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteAuditLogCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Audit log {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Audit log {id} deleted successfully" });
            });
        }
    }
}
