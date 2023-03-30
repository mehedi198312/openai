using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Error;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Models;
using App.Core.OpenAI.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace App.Core.OpenAI.Services.Implementations
{
    public class ModelsService : IModelsService
    {

        public ModelsService() { }

        public async Task<BaseResponse> Models(string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{baseUrl}/models");
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<ModelListResponseDto>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> Models(string token, string baseUrl, string id)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{baseUrl}/models/{id}");
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<ModelResponseDto>(resjson);
            return baseResponse;
        }

    }
}
