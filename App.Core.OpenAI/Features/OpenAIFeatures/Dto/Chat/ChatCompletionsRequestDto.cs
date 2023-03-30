using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Chat
{
    public class ChatCompletionsRequestDto
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("messages")]
        public List<ChatCompletionsMessagesRequestDto> Messages { get; set; }
    }
}
