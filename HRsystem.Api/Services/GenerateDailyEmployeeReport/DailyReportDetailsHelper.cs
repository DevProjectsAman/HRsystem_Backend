using HRsystem.Api.Database.DataTables;
using System.Text.Json;

namespace HRsystem.Api.Services.GenerateDailyEmployeeReport
{
    public static class DailyReportDetailsHelper
    {
        public static void AppendEvent(
            TbEmployeeMonthlyReport report,
            string type,
            Dictionary<string, object?> data)
        {
            DailyReportDetails details;

            if (string.IsNullOrWhiteSpace(report.Details))
            {
                details = new DailyReportDetails();
            }
            else
            {
                details = JsonSerializer.Deserialize<DailyReportDetails>(report.Details)
                          ?? new DailyReportDetails();
            }

            details.Events.Add(new DailyEvent
            {
                Type = type,
                Data = data
            });

            report.Details = JsonSerializer.Serialize(details);
        }
    
    
    
    
    
    
    
    
    }

    public class DailyReportDetails
    {
        public List<DailyEvent> Events { get; set; } = new();
    }

    public class DailyEvent
    {
        public string Type { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Flexible payload
        public Dictionary<string, object?> Data { get; set; } = new();
    }




}
