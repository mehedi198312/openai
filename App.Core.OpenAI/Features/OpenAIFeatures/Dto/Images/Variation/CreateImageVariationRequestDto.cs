using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Http;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Variation
{
    public class CreateImageVariationRequestDto
    {
        [JsonPropertyName("image")]
        public IFormFile Image { get; set; }

        [JsonPropertyName("n")]
        public int n { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }
    }

    public class CreateImageVariationRequestDto1
    {
        [JsonPropertyName("image")]
        public IFormFile Image { get; set; }

        [JsonPropertyName("n")]
        public int n { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }
    }
}
