using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Variation;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface IImageVariationService
    {
        Task<BaseResponse> CreateImageVariation(CreateImageVariationRequestDto request, string token, string baseUrl);
    }
}
