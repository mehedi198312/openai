using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings;
using App.Core.OpenAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenAIApp.Helpers;

namespace App.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EmbeddingsController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IEmbeddingsService _embeddingsService;

        public EmbeddingsController(IConfiguration configuration, IEmbeddingsService embeddingsService)
        {
            _configuration = configuration;
            _embeddingsService = embeddingsService;
        }

        [Authorize(Key.One)]
        [HttpPost("creates")]
        public async Task<IActionResult> CreatedEmbeddings(CreatedEmbeddingsRequestDto request)
        {

            #region "Sample Request"
            //{
            //    "model": "text-embedding-ada-002",
            //    "input": "The food was delicious and the waiter..."
            //}
            #endregion

            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _embeddingsService.CreatedEmbeddings(request, token, baseurl));
        }
        
    }

}