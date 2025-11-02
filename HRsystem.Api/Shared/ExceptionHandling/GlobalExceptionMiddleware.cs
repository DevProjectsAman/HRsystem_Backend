using HRsystem.Api.Shared.DTO;
using System.Net;
using System.Text.Json;
using FluentValidation;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var response = new ResponseResultDTO
            {
                Success = false,
                Message = "Validation failed",
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
        catch (Exception ex)
        {
            Console.WriteLine($"🔥 Unhandled exception: {ex}");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            string friendlyMessage = "An unexpected error occurred.";

            if (ex is Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateEx && dbUpdateEx.InnerException != null)
            {
                friendlyMessage = GetFriendlyMessage(dbUpdateEx.InnerException.Message);
            }

            var response = new ResponseResultDTO
            {
                Success = false,
                Message = friendlyMessage,
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
    }

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
