using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings
{
    public class EmbeddingsFileDto
    {
        [JsonPropertyName("project")]
        public string Project { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("fileId")]
        public string FileId { get; set; }

        [JsonPropertyName("file")]
        public IFormFile File { get; set; }

        [JsonPropertyName("embeddinglanguageModel")]
        public string? EmbeddingLanguageModel { get; set; }

        [JsonPropertyName("gptLanguageModel")]
        public string? GPTLanguageModel { get; set; }
    }

}
