using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Chat;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface IChatService
    {
        Task<BaseResponse> Completions(ChatCompletionsRequestDto request, string token, string baseUrl);
    }
}
