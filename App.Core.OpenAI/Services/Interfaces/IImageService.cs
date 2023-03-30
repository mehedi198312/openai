using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Create;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Edit;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface IImageService
    {
        Task<BaseResponse> CreateImage(CreateImageRequestDto request, string token, string baseUrl);
        Task<BaseResponse> EditImage(EditImageRequestDto request, string token, string baseUrl);
    }
}
