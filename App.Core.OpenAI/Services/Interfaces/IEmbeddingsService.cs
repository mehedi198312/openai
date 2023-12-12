using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Completions;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface IEmbeddingsService
    {
        Task<GeneratedEmbeddingsDto> CreateEmbeddings(EmbeddingsFileDto request, AppSettings appSettings);
        Task<AnswerFromVectorDto> QueryByVector(ChatCompletionsWithFileRequestDto searchEmbedding, AppSettings appSettings);
    }
}
