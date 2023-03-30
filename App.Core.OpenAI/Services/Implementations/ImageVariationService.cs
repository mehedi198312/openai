using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Error;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Variation;
using App.Core.OpenAI.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace App.Core.OpenAI.Services.Implementations
{
    public class ImageVariationService : IImageVariationService
    {

        public ImageVariationService() { }

        public async Task<BaseResponse> CreateImageVariation(CreateImageVariationRequestDto request, string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();
            var multipartContent = new MultipartFormDataContent();

            if (request.Size != null)
                multipartContent.Add(new StringContent(request.Size), "size");
            if (request.n != null)
                multipartContent.Add(new StringContent(request.n.ToString()!), "n");

            byte[] originalFile;
            using (var ms = new MemoryStream())
            {
                request.Image.CopyTo(ms);
                originalFile = ms.ToArray();
            }
            multipartContent.Add(new ByteArrayContent(originalFile), "image", request.Image.FileName);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"{baseUrl}/images/variations", multipartContent);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<CreateImageVariationResponseDto>(resjson);
            return baseResponse;
        }

    }
}
