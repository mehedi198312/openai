using App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.Create;
using App.Core.OpenAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FineTunesController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IFineTunesService _fineTunesService;

        public FineTunesController(IConfiguration configuration, IFineTunesService fineTunesService)
        {
            _configuration = configuration;
            _fineTunesService = fineTunesService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetFineTuneList()
        {
            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _fineTunesService.GetFineTuneList(token, baseurl));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateFineTunes(CreateFineTunesRequestDto request)
        {

            #region "Sample Request"
            //{
            //    "training_file": "file-XGinujblHPwGLSztz8cPS8XY"
            //}
            #endregion

            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _fineTunesService.CreateFineTune(request, token, baseurl));
        }

        [HttpGet("retrieve/{fineTuneId}")]
        public async Task<IActionResult> RetrieveFineTune(string fineTuneId)
        {
            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _fineTunesService.RetrieveFineTune(token, baseurl, fineTuneId));
        }

        [HttpPost("{fineTuneId}/cancel")]
        public async Task<IActionResult> CancelFineTune(string fineTuneId)
        {
            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _fineTunesService.CancelFineTune(token, baseurl, fineTuneId));
        }

        [HttpGet("{fineTuneId}/events")]
        public async Task<IActionResult> GetFineTuneEventList(string fineTuneId)
        {
            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _fineTunesService.GetFineTuneEventList(token, baseurl, fineTuneId));
        }

        [HttpDelete("model/delete/{fineTunedModel}")]
        public async Task<IActionResult> DeleteFineTuneModel(string fineTunedModel)
        {
            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _fineTunesService.DeleteFineTunedModel(token, baseurl, fineTunedModel));
        }        

    }

}