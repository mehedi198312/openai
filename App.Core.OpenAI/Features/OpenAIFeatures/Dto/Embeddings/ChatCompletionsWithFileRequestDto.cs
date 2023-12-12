using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Completions;
using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings
{
    public class ChatCompletionsWithFileRequestDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("project")]
        public string Project { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("fileId")]
        public string FileId { get; set; }

        [JsonPropertyName("fileName")]
        public string FileName { get; set; }

        [JsonPropertyName("gptModel")]
        public string? GPTModel { get; set; }

        [JsonPropertyName("embeddinglanguageModel")]
        public string? EmbeddingLanguageModel { get; set; }

        [JsonPropertyName("messages")]
        public List<CopywritingChatCompletionMessagesDto> Messages { get; set; }

        [JsonPropertyName("temperature")]
        public int Temperature { get; set; }

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonPropertyName("top_p")]
        public int TopP { get; set; }

        [JsonPropertyName("frequency_penalty")]
        public int FrequencyPenalty { get; set; }

        [JsonPropertyName("presence_penalty")]
        public int PresencePenalty { get; set; }
    }
}

public class ChatCompletionsWithFileRequestDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("project")]
    public string Project { get; set; }

    [JsonPropertyName("userId")]
    public string UserId { get; set; }

    [JsonPropertyName("fileId")]
    public string FileId { get; set; }

    [JsonPropertyName("fileName")]
    public string FileName { get; set; }

    [JsonPropertyName("askedOrSearch")]
    public string AskedOrSearch { get; set; }

    [JsonPropertyName("gptModel")]
    public string? GPTModel { get; set; }

    [JsonPropertyName("embeddinglanguageModel")]
    public string? EmbeddingLanguageModel { get; set; }

    [JsonPropertyName("temperature")]
    public int Temperature { get; set; }

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }

    [JsonPropertyName("top_p")]
    public int TopP { get; set; }

    [JsonPropertyName("frequency_penalty")]
    public int FrequencyPenalty { get; set; }

    [JsonPropertyName("presence_penalty")]
    public int PresencePenalty { get; set; }
}
