using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Chat;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Completions;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Error;
using App.Core.OpenAI.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace App.Core.OpenAI.Services.Implementations
{
    public class CompletionsService : ICompletionsService
    {

        public CompletionsService() { }

        public async Task<BaseResponse> Completions(CompletionsRequestDto request, string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseUrl}/completions", content);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<CompletionsResponseDto>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> ChatCompletions(ChatCompletionsRequestDto request, string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseUrl}/chat/completions", content);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<ChatCompletionsResponseDto>(resjson);
            return baseResponse;
        }

    }
}
