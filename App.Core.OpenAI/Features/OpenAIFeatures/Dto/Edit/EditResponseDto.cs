using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Edit;
using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Models
{
    public class EditResponseDto
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("choices")]
        public List<EditChoiceResponseDto> Choices { get; set; }

        [JsonPropertyName("usage")]
        public EditUsageResponseDto Usage { get; set; }
    }
}
