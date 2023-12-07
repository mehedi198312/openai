using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Images.Create
{
    public class CreateImageRequestDto
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("quality")]
        public string Quality { get; set; }

        [JsonPropertyName("style")]
        public string Style { get; set; }

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        [JsonPropertyName("n")]
        public int n { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }
    }
}
