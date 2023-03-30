using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.Common
{
    public class HyperParametersResponse
    {
        [JsonPropertyName("batch_size")]
        public int? BatchSize { get; set; }

        [JsonPropertyName("learning_rate_multiplier")]
        public float? LearningRateMultiplier { get; set; }

        [JsonPropertyName("n_epochs")]
        public int? NEpochs { get; set; }

        [JsonPropertyName("prompt_loss_weight")]
        public float? PromptLossWeight { get; set; }
    }
}
