using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface IModelsService
    {
        Task<BaseResponse> Models(string token, string baseUrl);
        Task<BaseResponse> Models(string token, string baseUrl, string id);
    }
}
