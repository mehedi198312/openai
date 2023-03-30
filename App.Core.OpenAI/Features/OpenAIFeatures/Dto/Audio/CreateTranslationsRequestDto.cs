using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Audio
{
    public class CreateTranslationsRequestDto
    {
        [JsonPropertyName("file")]
        public IFormFile File { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }
    }
}
