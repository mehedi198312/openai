using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Chat
{
    public class ChatCompletionsRequestDto
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("messages")]
        public List<ChatCompletionsMessagesRequestDto> Messages { get; set; }

        [JsonPropertyName("temperature")]
        public int Temperature { get; set; }

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
