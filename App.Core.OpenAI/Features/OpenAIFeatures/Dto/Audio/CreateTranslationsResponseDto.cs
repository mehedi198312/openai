using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Audio
{
    public class CreateTranslationsResponseDto
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
