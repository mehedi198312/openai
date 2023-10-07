using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings;
using Pinecone;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface IPineConeService
    {
        Task<BaseResponse> Upsert(List<float> Embedding, string chunk, EmbeddingsFileDto request, AppSettings appSettings);

        Task<BaseResponse> UpsertList(List<EmbeddingsDataDto> data, List<ChunkDto> chunks, EmbeddingsFileDto request, AppSettings appSettings);

        Task<BaseResponse> Fetch(string vector, AppSettings appSettings);

        Task<BaseResponse> QueryByVector(SearchEmbeddingDto searchEmbedding, List<float> vector, AppSettings appSettings);

        Task<BaseResponse> DeleteIndex(AppSettings appSettings);
    }
}
