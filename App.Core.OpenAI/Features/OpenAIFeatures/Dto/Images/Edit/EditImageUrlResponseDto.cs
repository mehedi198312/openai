using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Edit
{
    public class EditImageUrlResponseDto
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
