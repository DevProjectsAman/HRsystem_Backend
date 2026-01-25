using System.Text;
using System.Text.Json;

namespace HRsystem.Api.Helpers
{
    public class GeneralHelpers
    {

        public static string ExtractUsernameFromRequest(HttpContext context)
        {
            // EnableBuffering allows us to read the stream and reset it
            context.Request.EnableBuffering();

            using (var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true)) // Critical: leaveOpen must be true!
            {
                var body = reader.ReadToEndAsync().GetAwaiter().GetResult();

                // Reset the position so the Controller can read it again
                context.Request.Body.Position = 0;

                try
                {
                    // Parse the JSON (adjust "email" or "username" to match your DTO)
                    var json = JsonDocument.Parse(body);
                    if (json.RootElement.TryGetProperty("email", out var element))
                    {
                        return element.GetString();
                    }
                }
                catch
                {
                    return null; // Not a valid JSON or missing property
                }
            }

            return null;
        }
    }
}
