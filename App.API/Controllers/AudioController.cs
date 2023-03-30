using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Audio;
using App.Core.OpenAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AudioController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IAudioService _audioService;

        public AudioController(IConfiguration configuration, IAudioService audioService)
        {
            _configuration = configuration;
            _audioService = audioService;
        }

        [HttpPost("transcriptions")]
        public async Task<IActionResult> CreateTranscriptions([FromForm] CreateTranscriptionsRequestDto request)
        {

            #region "Sample Request"
            //{
            //    "file": "german.m4a",
            //    "model": "whisper-1"
            //}
            #endregion

            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _audioService.CreateTranscriptions(request, token, baseurl));
        }

        [HttpPost("translations")]
        public async Task<IActionResult> CreateTranslations([FromForm] CreateTranslationsRequestDto request)
        {

            #region "Sample Request"
            //{
            //    "file": "german.m4a",
            //    "model": "whisper-1"
            //}
            #endregion

            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _audioService.CreateTranslations(request, token, baseurl));
        }

    }

}