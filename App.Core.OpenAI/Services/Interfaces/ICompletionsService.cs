﻿using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Chat;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Completions;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface ICompletionsService
    {
        Task<BaseResponse> Completions(CompletionsRequestDto request, string token, string baseUrl);
        Task<BaseResponse> ChatCompletions(ChatCompletionsRequestDto request, string token, string baseUrl);
    }
}
