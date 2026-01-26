using HRsystem.Api.Shared.DTO;
using System.Net;
using System.Text.Json;
using FluentValidation;
using Serilog;
using Serilog.Formatting.Compact; // You will need this NuGet package

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;

        // UPDATED: Configure Serilog for Structured JSON
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext() // Adds contextual info to the JSON
            .WriteTo.File(
                formatter: new CompactJsonFormatter(), // Switched from template to JSON Formatter
                path: "Logs/log-.json",                // Changed extension to .json
                rollingInterval: RollingInterval.Day
            )
            .CreateLogger();
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            Log.Warning("Validation failed: {Message}", ex.Message);

            var statusCode = HttpStatusCode.BadRequest;
            await WriteResponseAsync(context, statusCode, "Validation failed", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            Log.Warning("Unauthorized access: {Message}", ex.Message);

            var statusCode = HttpStatusCode.Unauthorized;
            await WriteResponseAsync(context, statusCode, "Unauthorized access", ex);
        }
        catch (KeyNotFoundException ex)
        {
            Log.Warning("Resource not found: {Message}", ex.Message);

            var statusCode = HttpStatusCode.NotFound;
            await WriteResponseAsync(context, statusCode, "Resource not found", ex);
        }
        catch (Exception ex)
        {
            var severity = GetSeverity(ex);
            if (severity == "Critical")
                Log.Fatal(ex, "Critical exception occurred");
            else
                Log.Error(ex, "Unhandled exception occurred");

            var statusCode = HttpStatusCode.InternalServerError;
            string friendlyMessage = "An unexpected error occurred.";

            if (ex is Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateEx && dbUpdateEx.InnerException != null)
            {
                var dbMessage = dbUpdateEx.InnerException.Message;
                friendlyMessage = GetFriendlyMessage(dbMessage);

                if (dbMessage.Contains("FOREIGN KEY constraint fails", StringComparison.OrdinalIgnoreCase) ||
                    dbMessage.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase))
                {
                    statusCode = HttpStatusCode.Conflict;
                }
            }

            await WriteResponseAsync(context, statusCode, friendlyMessage, ex);
        }
    }

    // DRY: Helper to write the JSON response to the client
    private static async Task WriteResponseAsync(HttpContext context, HttpStatusCode code, string message, Exception ex)
    {
        context.Response.StatusCode = (int)code;
        context.Response.ContentType = "application/json";

        var response = new ResponseResultDTO
        {
            Success = false,
            StatusCode = (int)code,
            Message = message,
            Errors = new List<ResponseErrorDTO>
            {
                new ResponseErrorDTO
                {
                    Property = string.Empty,
                    Error = ex.InnerException?.Message ?? ex.Message
                }
            }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static string GetSeverity(Exception ex) =>
        ex is Microsoft.EntityFrameworkCore.DbUpdateException ? "Critical" : "Error";

    private static string GetFriendlyMessage(string dbMessage)
    {
        try
        {
            if (dbMessage.Contains("FOREIGN KEY constraint fails", StringComparison.OrdinalIgnoreCase))
            {
                if (dbMessage.Contains("TbEmployees", StringComparison.OrdinalIgnoreCase) &&
                    dbMessage.Contains("TbDepartments", StringComparison.OrdinalIgnoreCase))
                {
                    return "Cannot delete this department because employees are still assigned to it.";
                }

                return "This record cannot be deleted because it is referenced by other data.";
            }

            if (dbMessage.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase))
                return "A record with the same unique value already exists.";

            var match = System.Text.RegularExpressions.Regex.Match(
                dbMessage,
                @"fails \(`(?<db>.+?)`\.`(?<table>.+?)`, CONSTRAINT `(?<fk>.+?)` FOREIGN KEY \(`(?<column>.+?)`\) REFERENCES `(?<refTable>.+?)`"
            );

            if (match.Success)
            {
                var table = match.Groups["table"].Value;
                var refTable = match.Groups["refTable"].Value;

                string Clean(string name) =>
                    string.Join(" ", name.Split('_', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));

                return $"The record in **{Clean(table)}** cannot be deleted because related records exist in **{Clean(refTable)}**.";
            }
        }
        catch { }

        return "A database relationship error occurred. Please check related data.";
    }
}



