using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Error;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Create;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Edit;
using App.Core.OpenAI.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace App.Core.OpenAI.Services.Implementations
{
    public class ImageService : IImageService
    {

        public ImageService() { }

        public async Task<BaseResponse> CreateImage(CreateImageRequestDto request, string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseUrl}/images/generations", content);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
                return baseResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<CreateImageResponseDto>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> EditImage(EditImageRequestDto request, string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();
            var multipartContent = new MultipartFormDataContent();
           
            if (request.Size != null)
                multipartContent.Add(new StringContent(request.Size), "size");            
            if (request.n != null)
                multipartContent.Add(new StringContent(request.n.ToString()!), "n");            
            if (request.Mask != null)
            {
                byte[] maskFile;
                using (var ms = new MemoryStream())
                {
                    request.Mask.CopyTo(ms);
                    maskFile = ms.ToArray();
                }
                multipartContent.Add(new ByteArrayContent(maskFile), "mask", request.Mask.FileName);
            }

            multipartContent.Add(new StringContent(request.Prompt), "prompt");

            byte[] originalFile;
            using (var ms = new MemoryStream())
            {
                request.Image.CopyTo(ms);
                originalFile = ms.ToArray();
            }
            multipartContent.Add(new ByteArrayContent(originalFile), "image", request.Image.FileName);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"{baseUrl}/images/edits", multipartContent);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Message = errorResponse.Error.Message;
                return baseResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<EditImageResponseDto>(resjson);
            return baseResponse;
        }

    }
}
