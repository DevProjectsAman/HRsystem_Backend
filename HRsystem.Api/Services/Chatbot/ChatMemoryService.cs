using System.Collections.Concurrent;

namespace HRsystem.Api.Services.Chatbot
{
    public class ChatMemoryService
    {
        private readonly ConcurrentDictionary<string, List<string>> _memory = new();

        public void AddMessage(string sessionId, string message)
        {
            if (!_memory.ContainsKey(sessionId))
                _memory[sessionId] = new List<string>();

            _memory[sessionId].Add(message);

            // نخلي الذاكرة صغيرة - آخر 10 رسائل فقط
            if (_memory[sessionId].Count > 10)
                _memory[sessionId].RemoveAt(0);
        }

        public string GetContext(string sessionId)
        {
            if (_memory.TryGetValue(sessionId, out var messages))
                return string.Join("\n", messages);
            return "";
        }
    }
}
