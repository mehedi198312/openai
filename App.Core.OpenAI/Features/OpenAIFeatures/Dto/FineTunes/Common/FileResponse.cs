using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.Common
{
    public class FileResponse
    {
        [JsonPropertyName("bytes")]
        public int? Bytes { get; set; }
        [JsonPropertyName("filename")]
        public string FileName { get; set; }
        [JsonPropertyName("purpose")]
        public string Purpose { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("created_at")]
        public int CreatedAt { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
