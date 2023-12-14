using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Audio
{
    public class TextToSpeechRequestDto
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("input")]
        public string Input { get; set; }

        [JsonPropertyName("voice")]
        public string Voice { get; set; }

        [JsonPropertyName("response_format")]
        public string ResponseFormat { get; set; }       
    }
}
