using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Completions
{
    public class AnswerFromVectorDto
    {
        public bool IsSuccessful { get; set; }
        public CompletionsResponseDto CompletionsResponse { get; set; }
        public CreatedEmbeddingsResponseDto CreatedEmbeddingsResponse { get; set; }
        public List<int> NoOfPages { get; set; }
    }
}
