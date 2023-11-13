using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Completions
{
    public class GeneratedEmbeddingsDto
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public CreatedEmbeddingsResponseDto CreatedEmbeddingsResponse { get; set; }
    }
}
