using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface IEmbeddingsService
    {
        Task<BaseResponse> CreateEmbeddings(EmbeddingsFileDto request, AppSettings appSettings);
        Task<BaseResponse> QueryByVector(SearchEmbeddingDto searchEmbedding, AppSettings appSettings);
    }
}
