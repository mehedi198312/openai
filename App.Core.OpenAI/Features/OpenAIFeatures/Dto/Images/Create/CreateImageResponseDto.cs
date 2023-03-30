using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Create
{
    public class CreateImageResponseDto
    {
        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("data")]
        public List<CreatedImageDto> Data { get; set; }
    }
}
