using System.Collections.Generic;

namespace HRsystem.Api.Services.Chatbot
{
    public class IntentTemplate
    {
        public string Description { get; set; } = string.Empty;
        public List<string> Parameters { get; set; } = new();
        public string Sql { get; set; } = string.Empty;
        public List<string> Examples { get; set; } = new();  // ⬅️ إضافة Examples
    }
}