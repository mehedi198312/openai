using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Chat;
using App.Core.OpenAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenAIApp.Helpers;

namespace App.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IChatService _chatCompletionsService;

        public ChatController(IConfiguration configuration, IChatService chatCompletionsService)
        {
            _configuration = configuration;
            _chatCompletionsService = chatCompletionsService;
        }

        [Authorize(Key.One)]
        [HttpPost("completions")]
        public async Task<IActionResult> Completions(ChatCompletionsRequestDto request)
        {

            #region "Sample Request"
            //{
            //    "model": "gpt-3.5-turbo",
            //    "messages": [{ "role": "user", "content": "Hello!"}]
            //}
            #endregion

            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _chatCompletionsService.Completions(request, token, baseurl));
        }

    }

}