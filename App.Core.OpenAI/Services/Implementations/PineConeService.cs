using App.Core.OpenAI.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings;
using App.Core.OpenAI.Services.Interfaces;
using Pinecone;

namespace App.Core.OpenAI.Services.Implementations
{
    public class PineConeService : IPineConeService
    {

        public PineConeService() { }

        public async Task<BaseResponse> Upsert(List<float> Embedding, string chunk, EmbeddingsFileDto request, AppSettings appSettings)
        {
            var baseResponse = new BaseResponse();
            baseResponse.Message = MessageManager.PinecoreConnectionFail;

            // Initialize the client with your API key and environment
            using var pinecone = new PineconeClient(appSettings.PineConeAPIkey, appSettings.PineConeEnvironment);

            // List all indexes
            var indexes = await pinecone.ListIndexes();

            // Create a new index if it doesn't exist
            var indexName = appSettings.IndexName;
            if (!indexes.Contains(indexName))
            {
                await pinecone.CreateIndex(indexName, 1536, Metric.Cosine);
            }

            // Get an index by name
            using var index = await pinecone.GetIndex(indexName);

            // Assuming you have an instance of `index`
            // Create and upsert vectors

            var vectors = new[]
            {
                new Pinecone.Vector
                {
                    Id = Guid.NewGuid().ToString(),
                    Values = Embedding.ToArray(),
                    Metadata = new MetadataMap
                    {
                        ["project"] = request.Project,
                        ["userId"] = request.UserId,
                        ["fileId"] = request.FileId,
                        ["chunk"] = chunk
                    }
                }
            };
            await index.Upsert(vectors);

            baseResponse.IsSuccessful = true;
            //baseResponse.Data = vectors.FirstOrDefault().Id;
            baseResponse.Message = MessageManager.VectorSaveSuccessMessage;

            return baseResponse;
        }

        public async Task<BaseResponse> UpsertList(List<EmbeddingsDataDto> embeddingsData, List<ChunkDto> chunks, EmbeddingsFileDto request, AppSettings appSettings)
        {
            var baseResponse = new BaseResponse();
            baseResponse.Message = MessageManager.PinecoreConnectionFail;


            // Initialize the client with your API key and environment
            using var pinecone = new PineconeClient(appSettings.PineConeAPIkey, appSettings.PineConeEnvironment);

            // List all indexes
            var indexes = await pinecone.ListIndexes();

            // Create a new index if it doesn't exist
            var indexName = appSettings.IndexName;
            if (!indexes.Contains(indexName))
            {
                await pinecone.CreateIndex(indexName, 1536, Metric.Cosine);
            }

            // Get an index by name
            using var index = await pinecone.GetIndex(indexName);

            // Assuming you have an instance of `index`
            // Create and upsert vectors

            List<Pinecone.Vector> vectorList = new List<Pinecone.Vector>();
            int i = 0;
            foreach (var val in embeddingsData)
            {
                var vector = new Pinecone.Vector
                {
                    Id = Guid.NewGuid().ToString(),
                    Values = val.Embedding.ToArray(),
                    Metadata = new MetadataMap
                    {
                        ["project"] = request.Project,
                        ["userId"] = request.UserId,
                        ["fileId"] = request.FileId,
                        ["chunk"] = chunks[i].Chunk.ToString(),
                        ["pageNo"] = chunks[i].PageNo.ToString()
                    }
                };
                i= i + 1;
                vectorList.Add(vector);
            }

            await index.Upsert(vectorList);

            baseResponse.IsSuccessful = true;
            //baseResponse.Data = vectors.FirstOrDefault().Id;
            baseResponse.Message = MessageManager.VectorSaveSuccessMessage;

            return baseResponse;
        }

        public async Task<BaseResponse> Fetch(string vector, AppSettings appSettings)
        {
            var baseResponse = new BaseResponse();
            baseResponse.Message = MessageManager.PinecoreConnectionFail;

            // Initialize the client with your API key and environment
            using var pinecone = new PineconeClient(appSettings.PineConeAPIkey, appSettings.PineConeEnvironment);

            // List all indexes
            var indexes = await pinecone.ListIndexes();

            // Create a new index if it doesn't exist
            var indexName = appSettings.IndexName;
            if (!indexes.Contains(indexName))
            {
                baseResponse.IsSuccessful = false;
                baseResponse.Message = MessageManager.PinecoreIndexNotFound;
                return baseResponse;
            }

            // Get an index by name
            using var index = await pinecone.GetIndex(indexName);

            // Fetch vectors by IDs
            var fetched = await index.Fetch(new[] { vector });

            // Query scored vectors by ID
            var scored = await index.Query(vector, topK: 10);

            baseResponse.IsSuccessful = true;
            baseResponse.Data = fetched;
            baseResponse.Message = MessageManager.InformationFound;

            return baseResponse;
        }

        public async Task<BaseResponse> QueryByVector(SearchEmbeddingDto searchEmbedding, List<float> vector, AppSettings appSettings)
        {
            var baseResponse = new BaseResponse();
            baseResponse.Message = MessageManager.PinecoreConnectionFail;

            // Initialize the client with your API key and environment
            using var pinecone = new PineconeClient(appSettings.PineConeAPIkey, appSettings.PineConeEnvironment);

            // List all indexes
            var indexes = await pinecone.ListIndexes();

            // Create a new index if it doesn't exist
            var indexName = appSettings.IndexName;
            if (!indexes.Contains(indexName))
            {
                baseResponse.IsSuccessful = false;
                baseResponse.Message = MessageManager.PinecoreIndexNotFound;
                return baseResponse;
            }

            // Get an index by name
            using var index = await pinecone.GetIndex(indexName);

            var filter = new MetadataMap
            {
                ["project"] = new MetadataMap
                {
                    ["$in"] = new MetadataValue[] { searchEmbedding.Project }
                },

                ["userId"] = new MetadataMap
                {
                    ["$in"] = new MetadataValue[] { searchEmbedding.UserId }
                },

                ["fileId"] = new MetadataMap
                {
                    ["$in"] = new MetadataValue[] { searchEmbedding.FileId }
                }
            };

            var scored = await index.Query(vector.ToArray(), topK: appSettings.TopK, filter, includeMetadata: true);

            baseResponse.IsSuccessful = true;
            baseResponse.Data = scored;
            baseResponse.Message = MessageManager.InformationFound;

            return baseResponse;
        }

        public async Task<BaseResponse> DeleteIndex(AppSettings appSettings)
        {
            var baseResponse = new BaseResponse();
            baseResponse.Message = MessageManager.InformationFound;

            // Initialize the client with your API key and environment
            using var pinecone = new PineconeClient(appSettings.PineConeAPIkey, appSettings.PineConeEnvironment);

            // List all indexes
            var indexes = await pinecone.ListIndexes();

            // Create a new index if it doesn't exist
            var indexName = appSettings.IndexName;
            if (!indexes.Contains(indexName))
            {
                baseResponse.IsSuccessful = false;
                baseResponse.Message = MessageManager.PinecoreIndexNotFound;
                return baseResponse;
            }

            // Get an index by name
            using var index = await pinecone.GetIndex(indexName);

            // Delete an index
            await pinecone.DeleteIndex(indexName);

            baseResponse.IsSuccessful = true;
            baseResponse.Message = MessageManager.InformationDeleted;

            return baseResponse;
        }
        
    }
}
