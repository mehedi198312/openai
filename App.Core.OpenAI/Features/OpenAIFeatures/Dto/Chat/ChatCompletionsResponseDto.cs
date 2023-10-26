using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Chat
{
    public class ChatCompletionsResponseDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("choices")]
        public List<OpenAIChatCompletionsChoiceResponseDto> Choices { get; set; }

        [JsonPropertyName("usage")]
        public ChatCompletionsUsageDto Usage { get; set; }
    }    
}
