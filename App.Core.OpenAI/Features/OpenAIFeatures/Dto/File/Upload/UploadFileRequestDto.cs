using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.File.Upload
{
    public class UploadFileRequestDto
    {
        [JsonPropertyName("file")]
        public IFormFile File { get; set; }

        [JsonPropertyName("purpose")]
        public string Purpose { get; set; }
    }
}
