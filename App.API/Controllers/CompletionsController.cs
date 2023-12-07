using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Chat;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Completions;
using App.Core.OpenAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenAIApp.Helpers;

namespace App.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CompletionsController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly ICompletionsService _completionsService;

        public CompletionsController(IConfiguration configuration, ICompletionsService completionsService)
        {
            _configuration = configuration;
            _completionsService = completionsService;
        }

        [Authorize(Key.One)]
        [HttpPost()]
        public async Task<IActionResult> Completions(ChatCompletionsRequestDto request)
        {

            #region "Sample Request"
            //{
            //    "model": "text-davinci-002",
            //    "prompt": "vacation request email to boss",
            //    "temperature": 0.7,
            //    "max_tokens": 256,
            //    "top_p": 1,
            //    "frequency_penalty": 0,
            //    "presence_penalty": 0
            //}
            #endregion

            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _completionsService.ChatCompletions(request, token, baseurl));
        }
        
    }

}