using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings
{
    public class EmbeddingsDataDto
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("embedding")]
        public List<double> Embedding { get; set; }

        [JsonPropertyName("index")]
        public int? Index { get; set; }
    }
}
