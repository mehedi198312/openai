using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.File.List
{
    public class FileListResponseDto
    {
        [JsonPropertyName("data")]
        public List<FileDetailsResponseDto> Data { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }
    }
}
