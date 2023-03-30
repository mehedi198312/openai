﻿using System.Text.Json.Serialization;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.Common;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.FineTunes.Create
{

    public class CreateFineTunesResponseDto
    {
        [JsonPropertyName("events")]
        public List<EventResponse> Events { get; set; }

        [JsonPropertyName("fine_tuned_model")]
        public string FineTunedModel { get; set; }

        [JsonPropertyName("hyperparams")]
        public HyperParametersResponse HyperParams { get; set; }

        [JsonPropertyName("organization_id")]
        public string OrganizationId { get; set; }

        [JsonPropertyName("result_files")]
        public List<FileResponse> ResultFiles { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("validation_files")]
        public List<FileResponse> ValidationFiles { get; set; }

        [JsonPropertyName("training_files")]
        public List<FileResponse> TrainingFiles { get; set; }

        [JsonPropertyName("updated_at")]
        public int? UpdatedAt { get; set; }

        [JsonPropertyName("created_at")]
        public int CreatedAt { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }
    }

}
