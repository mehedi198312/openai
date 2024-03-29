﻿using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Models
{
    public class ModelResponseDto
    {

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("owned_by")]
        public string OwnedBy { get; set; }

        [JsonPropertyName("permission")]
        public List<PermissionResponseDto> Permission { get; set; }

    }
}
