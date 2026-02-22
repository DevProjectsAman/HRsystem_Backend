using System.Text.Json;

namespace HRsystem.Api.Services.RateLimitterMiddleware
{
    public class LoginBodyBufferingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginBodyBufferingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Only run this logic for the Login path
            if (context.Request.Path.StartsWithSegments("/api/auth/login") && context.Request.Method == "POST")
            {
                context.Request.EnableBuffering();

                // Read the body asynchronously
                using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0; // Reset for the Controller

                    try
                    {
                        using var json = JsonDocument.Parse(body);
                        if (json.RootElement.TryGetProperty("userName", out var element))
                        {
                            // Store it in Items for the Rate Limiter to find later
                            context.Items["LoginUserName"] = element.GetString();
                        }
                    }
                    catch { /* Ignore malformed JSON */ }
                }
            }

            await _next(context);
        }
    }
}
