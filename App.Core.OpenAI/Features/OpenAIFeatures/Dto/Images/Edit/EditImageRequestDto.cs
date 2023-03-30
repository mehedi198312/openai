using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Http;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Edit
{
    public class EditImageRequestDto
    {
        [JsonPropertyName("image")]
        public IFormFile Image { get; set; }

        [JsonPropertyName("mask")]
        public IFormFile? Mask { get; set; }

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        [JsonPropertyName("n")]
        public int n { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }
    }
}
