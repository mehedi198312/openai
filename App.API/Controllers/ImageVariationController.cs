using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Variation;
using App.Core.OpenAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenAIApp.Helpers;

namespace App.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageVariationController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IImageVariationService _imageVariation;

        public ImageVariationController(IConfiguration configuration, IImageVariationService imageVariation)
        {
            _configuration = configuration;
            _imageVariation = imageVariation;
        }

        [Authorize(Key.One)]
        [HttpPost("variations")]
        public async Task<IActionResult> CreateImageVariation(CreateImageVariationRequestDto1 request)
        {

            #region "Sample Request"
            //{
            //      "Image": "image_edit_original.png",
            //      "n": 1,
            //      "size": "1024x1024"
            //}
            #endregion

            string token = _configuration.GetSection("OpenAI").GetSection("APIkeys").Value;
            string baseurl = _configuration.GetSection("OpenAI").GetSection("BaseUrl").Value;

            return Ok();// await _imageVariation.CreateImageVariation(request, token, baseurl));

        }

    }

}