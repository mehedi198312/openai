using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Audio;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface IAudioService
    {
        Task<BaseResponse> CreateTranscriptions(CreateTranscriptionsRequestDto request, string token, string baseUrl);
        Task<BaseResponse> CreateTranslations(CreateTranslationsRequestDto request, string token, string baseUrl);
        Task<BaseResponse> TextToSpeech(TextToSpeechRequestDto request, AppSettings appSettings);
    }
}
