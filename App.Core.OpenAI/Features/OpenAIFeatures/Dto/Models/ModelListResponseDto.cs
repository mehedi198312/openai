using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Models
{
    public class ModelListResponseDto
    {
        [JsonPropertyName("data")]
        public List<ModelResponseDto> Data { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }
    }
}
