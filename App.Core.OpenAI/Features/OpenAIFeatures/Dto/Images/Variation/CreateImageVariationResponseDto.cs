using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Variation
{
    public class CreateImageVariationResponseDto
    {
        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("data")]
        public List<CreateImageVariationUrlResponseDto> Data { get; set; }
    }
}
