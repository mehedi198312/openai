using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Variation
{
    public class CreateImageVariationUrlResponseDto
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
