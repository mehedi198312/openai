using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings
{
    public class SearchEmbeddingDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("project")]
        public string Project { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("fileId")]
        public string FileId { get; set; }

        [JsonPropertyName("fileName")]
        public string FileName { get; set; }

        [JsonPropertyName("askedOrSearch")]
        public string AskedOrSearch { get; set; }
    }
}
