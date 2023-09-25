namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common
{
    public class AppSettings
    {
        public string OpenAiAPIkey { get; set; }
        public string OpenAiBaseUrl { get; set; }
        public string EmbeddingModel { get; set; }
        public string EmbeddingFile { get; set; }
        public int ChunkSize { get; set; }
        public int ChunkOverlap { get; set; }

        public string Model { get; set; }
        public int Temperature { get; set; }
        public int MaxTokens { get; set; }
        public int TopP { get; set; }
        public int FrequencyPenalty { get; set; }
        public int PresencePenalty { get; set; }



        public string PineConeAPIkey { get; set; }
        public string PineConeEnvironment { get; set; }
        public string IndexName { get; set; }
    }
}
