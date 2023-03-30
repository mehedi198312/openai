using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Completions
{
    public class CompletionsRequestDto
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        [JsonPropertyName("temperature")]
        public float Temperature { get; set; }

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonPropertyName("top_p")]
        public int TopP { get; set; }

        [JsonPropertyName("frequency_penalty")]
        public int FrequencyPenalty { get; set; }

        [JsonPropertyName("presence_penalty")]
        public int PresencePenalty { get; set; }
    }
}
