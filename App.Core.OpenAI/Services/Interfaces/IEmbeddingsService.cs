using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface IEmbeddingsService
    {
        Task<BaseResponse> CreatedEmbeddings(CreatedEmbeddingsRequestDto request, string token, string baseUrl);
    }
}
