using System.Text.Json.Serialization;

namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings
{
    public class ChunkDto
    {
        public int PageNo { get; set; }

        public string Chunk { get; set; }
    }
}
