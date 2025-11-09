using HRsystem.Api.Services.Chatbot;
using Microsoft.AspNetCore.Mvc;

namespace HRsystem.Api.Controllers
{
    [ApiController]
    [Route("api/chatbot")]
    public class ChatbotController : ControllerBase
    {
        private readonly ChatbotService _chatbotService;

        public ChatbotController(ChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            var response = await _chatbotService.AskAsync(request.Message);
            return Ok(response);
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }
}
