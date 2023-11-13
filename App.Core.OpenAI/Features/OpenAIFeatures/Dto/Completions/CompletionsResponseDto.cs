using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings;
using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Completions
{
    public class CompletionsResponseDto
    {
        [JsonPropertyName("id")]
        public string íd { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("choices")]
        public List<CompletionsChoiceResponseDto> Choices { get; set; }

        [JsonPropertyName("usage")]
        public CompletionsUsageResponseDto Usage { get; set; }
    }    
}
