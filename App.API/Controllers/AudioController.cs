using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Audio;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenAIApp.Helpers;

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

        [Authorize(Key.One)]
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

        [Authorize(Key.One)]
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


        [Authorize(Key.One)]
        [HttpPost("CreateTextToSpeech")]
        public async Task<IActionResult> CreateTextToSpeech(TextToSpeechRequestDto request)
        {

            #region "Sample Request"
            //{
            //"model": "tts-1",
            //"input": "Today is a wonderful day to build something people love!",
            //"voice": "alloy"
            //"response_format": "mp3"
            //}
            #endregion

            AppSettings appSettings = new AppSettings();

            appSettings.OpenAiAPIkey = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            appSettings.OpenAiBaseUrl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;
            appSettings.TTLFile = _configuration.GetSection("Audio").GetSection("TTLFile").Value;

            return Ok(await _audioService.TextToSpeech(request, appSettings));
        }
    }

}