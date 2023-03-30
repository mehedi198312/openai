using App.Core.OpenAI.Features.OpenAIFeatures.Dto.File.Upload;
using App.Core.OpenAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;

        public FileController(IConfiguration configuration, IFileService fileService)
        {
            _configuration = configuration;
            _fileService = fileService;
        }

        [HttpPost("list")]
        public async Task<IActionResult> FileList()
        {
            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _fileService.FileList(token, baseurl));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileRequestDto request)
        {

            #region "Sample Request"
            //{
            //    "model": "text-embedding-ada-002",
            //    "input": "The food was delicious and the waiter..."
            //}
            #endregion

            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _fileService.UploadFile(request, token, baseurl));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFile(string id)
        {
            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _fileService.DeleteFile(token, baseurl, id));
        }

        [HttpPost("retrieve/{id}")]
        public async Task<IActionResult> RetrieveFile(string id)
        {
            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _fileService.RetrieveFile(token, baseurl, id));
        }

        [HttpPost("retrieve/{id}/content")]
        public async Task<IActionResult> RetrieveFileContent(string id)
        {
            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _fileService.RetrieveFileContent(token, baseurl, id));
        }

    }

}