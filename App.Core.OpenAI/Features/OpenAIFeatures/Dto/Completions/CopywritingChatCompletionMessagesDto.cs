using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Completions
{
    public class CopywritingChatCompletionMessagesDto
    {
        public CopywritingChatCompletionMessagesDto()
        {
        }
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
