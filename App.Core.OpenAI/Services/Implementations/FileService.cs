using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Error;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.File.Delete;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.File.List;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.File.Retrieve;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.File.Upload;
using App.Core.OpenAI.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace App.Core.OpenAI.Services.Implementations
{
    public class FileService : IFileService
    {

        public FileService() { }

        public async Task<BaseResponse> FileList(string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{baseUrl}/files");
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<FileListResponseDto>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> UploadFile(UploadFileRequestDto request, string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();
            var multipartContent = new MultipartFormDataContent();

            byte[] file;
            using (var ms = new MemoryStream())
            {
                request.File.CopyTo(ms);
                file = ms.ToArray();
            }
            multipartContent.Add(new ByteArrayContent(file), "file", request.File.FileName);
            multipartContent.Add(new StringContent(request.Purpose), "purpose");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"{baseUrl}/files", multipartContent);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<UploadFileResponseDto>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> DeleteFile(string token, string baseUrl, string id)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"{baseUrl}/files/{id}");
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<DeleteFileResponseDto>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> RetrieveFile(string token, string baseUrl, string id)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{baseUrl}/files/{id}");
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<RetrieveFileResponseDto>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> RetrieveFileContent(string token, string baseUrl, string id)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{baseUrl}/files/{id}/content");
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }
            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<RetrieveFileResponseDto>(resjson);
            return baseResponse;
        }

    }
}
