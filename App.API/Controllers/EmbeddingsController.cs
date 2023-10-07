using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
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
        [HttpPost("CreatedEmbeddings")]
        public async Task<IActionResult> CreatedEmbeddings([FromForm] EmbeddingsFileDto request)
        {

            #region "Sample Request"
            //{
            //    "model": "text-embedding-ada-002",
            //    "input": "The food was delicious and the waiter..."
            //}
            #endregion

            AppSettings appSettings = new AppSettings();

            appSettings.OpenAiAPIkey = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            appSettings.OpenAiBaseUrl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            appSettings.EmbeddingModel = _configuration.GetSection("OpenAI").GetSection("EmbeddingModel").Value;
            appSettings.EmbeddingFile = _configuration.GetSection("SearchFile").GetSection("EmbeddingFile").Value;
            appSettings.ChunkSize = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("ChunkSize").Value.ToString());
            appSettings.ChunkOverlap = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("ChunkOverlap").Value.ToString());
            appSettings.Model = _configuration.GetSection("SearchFile").GetSection("Model").Value.ToString();
            appSettings.Temperature = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("Temperature").Value.ToString());
            appSettings.MaxTokens = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("MaxTokens").Value.ToString());
            appSettings.TopP = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("TopP").Value.ToString());
            appSettings.FrequencyPenalty = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("FrequencyPenalty").Value.ToString());
            appSettings.PresencePenalty = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("PresencePenalty").Value.ToString());

            appSettings.PineConeAPIkey = _configuration.GetSection("PineCone").GetSection("APIkey").Value;
            appSettings.PineConeEnvironment = _configuration.GetSection("PineCone").GetSection("Environment").Value;
            appSettings.IndexName = _configuration.GetSection("PineCone").GetSection("IndexName").Value;

            return Ok(await _embeddingsService.CreateEmbeddings(request, appSettings));
        }

        [Authorize(Key.One)]
        [HttpPost("QueryByVector")]
        public async Task<IActionResult> QueryByVector(SearchEmbeddingDto searchEmbedding)
        {
            AppSettings appSettings = new AppSettings();

            appSettings.OpenAiAPIkey = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            appSettings.OpenAiBaseUrl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            appSettings.EmbeddingModel = _configuration.GetSection("OpenAI").GetSection("EmbeddingModel").Value;
            appSettings.EmbeddingFile = _configuration.GetSection("SearchFile").GetSection("EmbeddingFile").Value;
            appSettings.ChunkSize = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("ChunkSize").Value.ToString());
            appSettings.ChunkOverlap = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("ChunkOverlap").Value.ToString());
            appSettings.Model = _configuration.GetSection("SearchFile").GetSection("Model").Value.ToString();
            appSettings.Temperature = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("Temperature").Value.ToString());
            appSettings.MaxTokens = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("MaxTokens").Value.ToString());
            appSettings.TopP = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("TopP").Value.ToString());
            appSettings.FrequencyPenalty = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("FrequencyPenalty").Value.ToString());
            appSettings.PresencePenalty = Convert.ToUInt16(_configuration.GetSection("SearchFile").GetSection("PresencePenalty").Value.ToString());

            appSettings.PineConeAPIkey = _configuration.GetSection("PineCone").GetSection("APIkey").Value;
            appSettings.PineConeEnvironment = _configuration.GetSection("PineCone").GetSection("Environment").Value;
            appSettings.IndexName = _configuration.GetSection("PineCone").GetSection("IndexName").Value;
            appSettings.TopK = Convert.ToUInt16(_configuration.GetSection("PineCone").GetSection("TopK").Value);

            return Ok(await _embeddingsService.QueryByVector(searchEmbedding, appSettings));
        }

    }

}