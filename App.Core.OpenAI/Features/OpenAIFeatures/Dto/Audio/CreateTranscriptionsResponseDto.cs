using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Audio
{
    public class CreateTranscriptionsResponseDto
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
