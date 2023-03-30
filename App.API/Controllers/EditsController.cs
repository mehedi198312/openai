using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Models;
using App.Core.OpenAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EditsController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IEditsService _editsService;

        public EditsController(IConfiguration configuration, IEditsService editsService)
        {
            _configuration = configuration;
            _editsService = editsService;
        }
        
        [HttpPost()]
        public async Task<IActionResult> Edits(EditRequestDto request)
        {
            #region "Sample Request"
            //{
            //    "model": "text-davinci-edit-001",
            //    "input": "What day of the wek is it?",
            //    "instruction": "Fix the spelling mistakes"
            //}
            #endregion

            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _editsService.Edits(request, token, baseurl));
        }

    }

}