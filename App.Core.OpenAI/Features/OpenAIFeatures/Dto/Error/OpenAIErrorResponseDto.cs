using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Error
{
    public class OpenAIErrorResponseDto
    {
        [JsonPropertyName("error")]
        public OpenAIErrorDto Error { get; set; }
    }
}
