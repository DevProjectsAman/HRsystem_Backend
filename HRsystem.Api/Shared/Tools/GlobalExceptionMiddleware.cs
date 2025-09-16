
// File: Middlewares/GlobalExceptionMiddleware.cs

using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace HRsystem.Api.Shared.Tools
{
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
                await _next(context); // run the next middleware
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    message = "Validation failed",
                    errors = ex.Errors.Select(e => e.ErrorMessage).ToList()
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
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

                var response = new
                {
                    success = false,
                    message = friendlyMessage,
                    error = ex.InnerException?.Message ?? ex.Message // keep raw for devs
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }



        private static string GetFriendlyMessage(string dbMessage)
        {
            try
            {
                // Match the MySQL FK error structure
                var match = System.Text.RegularExpressions.Regex.Match(
                    dbMessage,
                    @"fails \(`(?<db>.+?)`\.`(?<table>.+?)`, CONSTRAINT `(?<fk>.+?)` FOREIGN KEY \(`(?<column>.+?)`\) REFERENCES `(?<refTable>.+?)`"
                );

                if (match.Success)
                {
                    var table = match.Groups["table"].Value;
                    var column = match.Groups["column"].Value;
                    var refTable = match.Groups["refTable"].Value;

                    // Convert snake_case to nicer words
                    string Clean(string name) =>
                        string.Join(" ", name.Split('_', StringSplitOptions.RemoveEmptyEntries)
                                              .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));

                    return $"The value for **{Clean(column)}** in **{Clean(table)}** must match an existing record in **{Clean(refTable)}**.";
                }
            }
            catch
            {
                // fallback
            }

            return "A database relationship error occurred. Please check related data.";
        }


    }
}


