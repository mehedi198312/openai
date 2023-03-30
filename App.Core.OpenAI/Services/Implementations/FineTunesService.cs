using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Error;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.Create;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.Delete;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.List;
using App.Core.OpenAI.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace App.Core.OpenAI.Services.Implementations
{
    public class FineTunesService : IFineTunesService
    {

        public FineTunesService() { }

        public async Task<BaseResponse> CreateFineTune(CreateFineTunesRequestDto request, string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseUrl}/fine-tunes", content);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<CreateFineTunesResponseDto>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> GetFineTuneList(string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{baseUrl}/fine-tunes");
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<FineTuneListResponse>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> RetrieveFineTune(string token, string baseUrl, string fineTuneId)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{baseUrl}/fine-tunes/{fineTuneId}");
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<FineTuneResponse>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> CancelFineTune(string token, string baseUrl, string fineTuneId)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"{baseUrl}/fine-tunes/{fineTuneId}/cancel", null);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<FineTuneResponse>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> GetFineTuneEventList(string token, string baseUrl, string fineTuneId)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{baseUrl}/fine-tunes/{fineTuneId}/events");
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                throw new System.Exception(errorResponse.Error.Message);
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<FineTuneEventsResponse>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> DeleteFineTunedModel(string token, string baseUrl, string fineTunedModel)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"{baseUrl}/models/{fineTunedModel}");
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<FineTuneDeleteResponse>(resjson);
            return baseResponse;
        }

    }
}
