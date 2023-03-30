using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Error;
using App.Core.OpenAI.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace App.Core.OpenAI.Services.Implementations
{
    public class EmbeddingsService : IEmbeddingsService
    {

        public EmbeddingsService() { }

        public async Task<BaseResponse> CreatedEmbeddings(CreatedEmbeddingsRequestDto request, string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseUrl}/embeddings", content);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<CreatedEmbeddingsResponseDto>(resjson);
            return baseResponse;
        }

    }
}
