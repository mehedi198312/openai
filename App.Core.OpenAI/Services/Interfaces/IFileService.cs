using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.File.Delete;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.File.List;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.File.Retrieve;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.File.Upload;

namespace App.Core.OpenAI.Services.Interfaces
{
    public interface IFileService
    {
        Task<BaseResponse> FileList(string token, string baseUrl);
        Task<BaseResponse> UploadFile(UploadFileRequestDto request, string token, string baseUrl);
        Task<BaseResponse> DeleteFile(string token, string baseUrl, string id);
        Task<BaseResponse> RetrieveFile(string token, string baseUrl, string id);
        Task<BaseResponse> RetrieveFileContent(string token, string baseUrl, string id);
    }
}
