using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Create;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Edit;
using App.Core.OpenAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenAIApp.Helpers;

namespace App.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IImageService _imageService;

        public ImageController(IConfiguration configuration, IImageService imageService)
        {
            _configuration = configuration;
            _imageService = imageService;
        }

        [Authorize(Key.One)]
        [HttpPost("creates")]
        public async Task<IActionResult> ImagesCreates(CreateImageRequestDto request)
        {
            #region "Sample Request"
            //{
            //    "prompt": "A cute baby sea otter",
            //    "n": 2,
            //    "size": "1024x1024"
            //}
            #endregion

            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _imageService.CreateImage(request, token, baseurl));
        }

        [Authorize(Key.One)]
        [HttpPost("edits")]
        public async Task<IActionResult> ImagesEdits([FromForm] EditImageRequestDto request)
        {

            #region "Sample Request"
            //{
            //      "Image": "image_edit_original.png",
            //      "Mask": "image_edit_mask.png",
            //      "prompt": "A sunlit indoor lounge area with a pool containing a cat",
            //      "n": 1,
            //      "size": "1024x1024"
            //}
            #endregion

            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok(await _imageService.EditImage(request, token, baseurl));            
        }

    }

}