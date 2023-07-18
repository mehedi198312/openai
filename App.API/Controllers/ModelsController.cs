using App.Core.OpenAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenAIApp.Helpers;

namespace App.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModelsController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IModelsService _modelsService;

        public ModelsController(IConfiguration configuration, IModelsService modelsService)
        {
            _configuration = configuration;
            _modelsService = modelsService;
        }

        [Authorize(Key.One)]
        [HttpGet()]
        public async Task<IActionResult> Models()
        {
            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;
            return Ok(await _modelsService.Models(token, baseurl));
        }

        [Authorize(Key.One)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Models(string id)
        {
            #region "Sample Request"
            //id="text-davinci-003";
            #endregion

            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;
            return Ok(await _modelsService.Models(token, baseurl, id));
        }

    }

}