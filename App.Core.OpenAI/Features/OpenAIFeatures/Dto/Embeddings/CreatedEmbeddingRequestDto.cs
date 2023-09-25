using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings
{
    public class CreatedEmbeddingRequestDto
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("input")]
        public string Input { get; set; }
    }

}
