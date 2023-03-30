using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Models;
using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Edit
{
    public class EditChoiceResponseDto
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("usage")]
        public EditUsageResponseDto Usage { get; set; }
    }
}
