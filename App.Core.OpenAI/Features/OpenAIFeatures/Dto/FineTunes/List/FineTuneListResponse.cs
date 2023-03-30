using System.Text.Json.Serialization;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.Common;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.List
{
    public class FineTuneListResponse
    {
        [JsonPropertyName("data")] 
        public List<FineTuneResponse> Data { get; set; }
    }
}
