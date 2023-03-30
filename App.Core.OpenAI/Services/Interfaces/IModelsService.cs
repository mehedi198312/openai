using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Models;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface IModelsService
    {
        Task<BaseResponse> Models(string token, string baseUrl);
        Task<BaseResponse> Models(string token, string baseUrl, string id);
    }
}
