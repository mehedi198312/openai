using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Audio;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Error;
using App.Core.OpenAI.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace App.Core.OpenAI.Services.Implementations
{
    public class AudioService : IAudioService
    {

        public AudioService() { }

        public async Task<BaseResponse> CreateTranscriptions(CreateTranscriptionsRequestDto request, string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();
            var multipartContent = new MultipartFormDataContent();

            byte[] audioFile;
            using (var ms = new MemoryStream())
            {
                request.File.CopyTo(ms);
                audioFile = ms.ToArray();
            }
            multipartContent.Add(new ByteArrayContent(audioFile), "file", request.File.FileName);
            multipartContent.Add(new StringContent(request.Model), "model");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"{baseUrl}/audio/transcriptions", multipartContent);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }

            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<CreateTranscriptionsResponseDto>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> CreateTranslations(CreateTranslationsRequestDto request, string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();
            var multipartContent = new MultipartFormDataContent();

            byte[] audioFile;
            using (var ms = new MemoryStream())
            {
                request.File.CopyTo(ms);
                audioFile = ms.ToArray();
            }
            multipartContent.Add(new ByteArrayContent(audioFile), "file", request.File.FileName);
            multipartContent.Add(new StringContent(request.Model), "model");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"{baseUrl}/audio/translations", multipartContent);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }

            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<CreateTranslationsResponseDto>(resjson);
            return baseResponse;
        }

    }
}
