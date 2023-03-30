using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Create
{
    public class CreatedImageDto
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
