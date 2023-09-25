using App.Core.OpenAI.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Chat;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Completions;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Embeddings;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Error;
using App.Core.OpenAI.Services.Interfaces;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Pinecone;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace App.Core.OpenAI.Services.Implementations
{
    public class EmbeddingsService : IEmbeddingsService
    {

        private readonly IPineConeService _pineConeService;
        private readonly ICompletionsService _completionsService;

        public EmbeddingsService(IPineConeService pineConeService, ICompletionsService completionsService)
        {
            _pineConeService = pineConeService;
            _completionsService = completionsService;
        }

        #region "Private methods"

        private async Task<BaseResponse> UploadFile(EmbeddingsFileDto fileInfo, string fileFolder)
        {
            var baseResponse = new BaseResponse();
            baseResponse.Message = MessageManager.FileUploadFailed;

            var fileExtension = System.IO.Path.GetExtension(fileInfo.File.FileName);
            var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), fileFolder, $"{fileInfo.FileId}{fileExtension}");

            using (var stream = System.IO.File.Create(path))
            {
                await fileInfo.File.CopyToAsync(stream);
            }

            baseResponse.IsSuccessful = true;
            baseResponse.Data = path;
            baseResponse.Message = MessageManager.FileUploadSuccessfully;

            return baseResponse;
        }

        private async Task<BaseResponse> CreateChunk(string path, int chunkSize)
        {
            var baseResponse = new BaseResponse();

            PdfReader reader = new PdfReader(path);
            baseResponse.Message = MessageManager.ChunkCreateFailed;

            string text = string.Empty;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                text += PdfTextExtractor.GetTextFromPage(reader, page);
            }
            reader.Close();

            List<string> chunks = text.ToString().Chunk(chunkSize).Select(x => new string(x)).ToList();

            baseResponse.IsSuccessful = true;
            baseResponse.Data = chunks;
            baseResponse.Message = MessageManager.ChunkCreateSuccessfully;

            return baseResponse;
        }

        private async Task<BaseResponse> CreateEmbedding(AppSettings appSettings, string chunk)
        {
            var baseResponse = new BaseResponse();
            baseResponse.Message = MessageManager.EmbeddingCreateFailed;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.OpenAiAPIkey);

            CreatedEmbeddingRequestDto createdEmbeddingRequestDto = new CreatedEmbeddingRequestDto();
            createdEmbeddingRequestDto.Model = appSettings.EmbeddingModel;
            createdEmbeddingRequestDto.Input = chunk;

            var json = JsonSerializer.Serialize(createdEmbeddingRequestDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{appSettings.OpenAiBaseUrl}/embeddings", content);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }

            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<CreatedEmbeddingsResponseDto>(resjson);
            baseResponse.Message = MessageManager.EmbeddingCreateSuccessfully;

            return baseResponse;
        }

        private async Task<BaseResponse> CreateEmbeddings(AppSettings appSettings, List<string> chunks)
        {
            var baseResponse = new BaseResponse();
            baseResponse.Message = MessageManager.EmbeddingCreateFailed;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.OpenAiAPIkey);

            CreatedEmbeddingsRequestDto createdEmbeddingsRequestDto = new CreatedEmbeddingsRequestDto();
            createdEmbeddingsRequestDto.Model = appSettings.EmbeddingModel;
            createdEmbeddingsRequestDto.Input = chunks;

            var json = JsonSerializer.Serialize(createdEmbeddingsRequestDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{appSettings.OpenAiBaseUrl}/embeddings", content);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }

            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<CreatedEmbeddingsResponseDto>(resjson);
            baseResponse.Message = MessageManager.EmbeddingCreateSuccessfully;

            return baseResponse;
        }

        #endregion

        public async Task<BaseResponse> CreateEmbeddings(EmbeddingsFileDto embeddingsFile, AppSettings appSettings)
        {
            var fileUploadResponse = await UploadFile(embeddingsFile, appSettings.EmbeddingFile);            

            if (!fileUploadResponse.IsSuccessful)
                return fileUploadResponse;

            var createChunkReponse = await CreateChunk(fileUploadResponse.Data.ToString(), appSettings.ChunkSize);

            if(!createChunkReponse.IsSuccessful)
                return createChunkReponse;

            var createEmbeddingsResponse = await CreateEmbeddings(appSettings, ((List<string>)createChunkReponse.Data));
            if (createEmbeddingsResponse.IsSuccessful)
            {
                var embeddingsData = (CreatedEmbeddingsResponseDto)createEmbeddingsResponse.Data;
                await _pineConeService.UpsertList(embeddingsData.Data, ((List<string>)createChunkReponse.Data), embeddingsFile, appSettings);
            }

            var baseResponse = new BaseResponse();
            baseResponse.IsSuccessful = true;
            baseResponse.Message = MessageManager.EmbeddingCreateSuccessfully;
            return baseResponse;
        }

        public async Task<BaseResponse> QueryByVector(SearchEmbeddingDto searchEmbedding, AppSettings appSettings)
        {
            var baseResponse = new BaseResponse();

            //Create search string embedding
            var searchStringEmbedding = await CreateEmbedding(appSettings, searchEmbedding.AskedOrSearch);
            if (!searchStringEmbedding.IsSuccessful)
                return searchStringEmbedding;

            //Search by vector
            var searchStringVector = ((CreatedEmbeddingsResponseDto)searchStringEmbedding.Data).Data.FirstOrDefault().Embedding;
            var scoredVectors = await _pineConeService.QueryByVector(searchEmbedding, searchStringVector, appSettings);
            if (!scoredVectors.IsSuccessful)
                return scoredVectors;

            string filterredString = "";
            foreach ( var scoredVector in (ScoredVector[])scoredVectors.Data)
            {
                filterredString += scoredVector.Metadata.FirstOrDefault().Value.Inner;
            }

            //Get search result from filtered context which is get by vector search.
            CompletionsRequestDto completionsRequestDto = new CompletionsRequestDto();
            completionsRequestDto.Model = appSettings.Model;// "text-davinci-003";
            completionsRequestDto.Temperature = appSettings.Temperature;//  0;
            completionsRequestDto.MaxTokens = appSettings.MaxTokens;//  150;
            completionsRequestDto.TopP = appSettings.TopP;//  1;
            completionsRequestDto.FrequencyPenalty = appSettings.FrequencyPenalty;//  0;
            completionsRequestDto.PresencePenalty = appSettings.PresencePenalty;//  0;

            completionsRequestDto.Prompt = "Answer the question based on the context below, " +
                "and if the question can't be answered based on the context, say \"I don't know\"\n\n" +
                "Context: " + filterredString + "\n\n---\n\n" +
                "Question: " + searchEmbedding.AskedOrSearch + "\n" +
                "Answer:";

            baseResponse = await _completionsService.Completions(completionsRequestDto, appSettings.OpenAiAPIkey, appSettings.OpenAiBaseUrl);            
            return baseResponse;

        }

    }
}
