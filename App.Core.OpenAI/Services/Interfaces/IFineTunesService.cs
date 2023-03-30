using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.Create;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.Delete;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.List;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface IFineTunesService
    {
        Task<BaseResponse> CreateFineTune(CreateFineTunesRequestDto request, string token, string baseUrl);
        Task<BaseResponse> GetFineTuneList(string token, string baseUrl);
        Task<BaseResponse> RetrieveFineTune(string token, string baseUrl, string fineTuneId);
        Task<BaseResponse> CancelFineTune(string token, string baseUrl, string fineTuneId);
        Task<BaseResponse> GetFineTuneEventList(string token, string baseUrl, string fineTuneId);
        Task<BaseResponse> DeleteFineTunedModel(string token, string baseUrl, string fineTunedModel);
    }
}
