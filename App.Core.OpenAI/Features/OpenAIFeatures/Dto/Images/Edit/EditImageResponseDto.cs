using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Edit
{
    public class EditImageResponseDto
    {
        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("data")]
        public List<EditImageUrlResponseDto> Data { get; set; }
    }
}
