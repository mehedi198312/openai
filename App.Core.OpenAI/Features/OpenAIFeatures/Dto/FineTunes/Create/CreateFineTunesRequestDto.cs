using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.Create
{
    public class CreateFineTunesRequestDto
    {
        [JsonPropertyName("training_file")]
        public string TrainingFile { get; set; }
    }
}
