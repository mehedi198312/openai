using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Models;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface IEditsService
    {
        Task<BaseResponse> Edits(EditRequestDto request, string token, string baseUrl);
    }
}
