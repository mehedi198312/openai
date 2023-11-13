using App.Core.OpenAI.Common;
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

        private async Task<BaseResponse> CreateChunk(string path, int chunkSize, int overLapping)
        {
            var baseResponse = new BaseResponse();
            baseResponse.Message = MessageManager.ChunkCreateFailed;            
            List<ChunkDto> Chunks = new List<ChunkDto>();

            PdfReader reader = new PdfReader(path);
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                var textByPage = PdfTextExtractor.GetTextFromPage(reader, page);
                List<string> chunksByPage = textByPage.ToString().Chunk(chunkSize).Select(x => new string(x)).ToList();
                for(int i=0; i< chunksByPage.Count; i++)
                {
                    ChunkDto chunk = new ChunkDto();
                    chunk.PageNo = page;
                    string overlapString = (i > 0)? chunksByPage[i - 1].ToString().Substring(chunksByPage[i - 1].ToString().Length-overLapping, overLapping):"";
                    chunk.Chunk = $"{overlapString} {chunksByPage[i]}";
                    Chunks.Add(chunk);
                }
            }
            reader.Close();

            #region "Old Logic"

            //string text = string.Empty;
            //for (int page = 1; page <= reader.NumberOfPages; page++)
            //{
            //    text += PdfTextExtractor.GetTextFromPage(reader, page);
            //}
            //reader.Close();

            //List<string> chunks = text.ToString().Chunk(chunkSize).Select(x => new string(x)).ToList();

            #endregion

            baseResponse.IsSuccessful = true;
            baseResponse.Data = Chunks;
            baseResponse.Message = MessageManager.ChunkCreateSuccessfully;

            return baseResponse;
        }

        private async Task<BaseResponse> CreateEmbedding(AppSettings appSettings, string languageModel, string chunk)
        {
            var baseResponse = new BaseResponse();
            baseResponse.Message = MessageManager.EmbeddingCreateFailed;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.OpenAiAPIkey);

            CreatedEmbeddingRequestDto createdEmbeddingRequestDto = new CreatedEmbeddingRequestDto();
            createdEmbeddingRequestDto.Model = languageModel;
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

        private async Task<BaseResponse> CreateEmbeddings(AppSettings appSettings, string languageModel, List<ChunkDto> chunks)
        {
            var baseResponse = new BaseResponse();
            baseResponse.Message = MessageManager.EmbeddingCreateFailed;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.OpenAiAPIkey);

            CreatedEmbeddingsRequestDto createdEmbeddingsRequestDto = new CreatedEmbeddingsRequestDto();
            createdEmbeddingsRequestDto.Model = languageModel;
            createdEmbeddingsRequestDto.Input = chunks.Select(x => x.Chunk).ToList();

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

        public async Task<GeneratedEmbeddingsDto> CreateEmbeddings(EmbeddingsFileDto embeddingsFile, AppSettings appSettings)
        {
            GeneratedEmbeddingsDto generatedEmbeddingsDto = new GeneratedEmbeddingsDto();
            generatedEmbeddingsDto.IsSuccessful = false;

            //Upload file
            var fileUploadResponse = await UploadFile(embeddingsFile, appSettings.EmbeddingFile);            
            if (!fileUploadResponse.IsSuccessful)
                generatedEmbeddingsDto.Message = fileUploadResponse.Message;

            //Create chunk for embedding file
            var createChunkReponse = await CreateChunk(fileUploadResponse.Data.ToString(), appSettings.ChunkSize, appSettings.ChunkOverlap);
            if(!createChunkReponse.IsSuccessful)
                generatedEmbeddingsDto.Message = createChunkReponse.Message;

            //Generate embedding/vector using OpenAI embedding API
            var createEmbeddingsResponse = await CreateEmbeddings(appSettings, embeddingsFile.EmbeddingLanguageModel, ((List<ChunkDto>)createChunkReponse.Data));
            if (createEmbeddingsResponse.IsSuccessful)
            {
                //Save vector into pinecone vector database
                var createdEmbeddingsResponseDto = (CreatedEmbeddingsResponseDto)createEmbeddingsResponse.Data;
                await _pineConeService.UpsertList(createdEmbeddingsResponseDto.Data, ((List<ChunkDto>)createChunkReponse.Data), embeddingsFile, appSettings);
            
                //return response
                generatedEmbeddingsDto.CreatedEmbeddingsResponse = createdEmbeddingsResponseDto;
                generatedEmbeddingsDto.IsSuccessful = true;
            }

            return generatedEmbeddingsDto;
        }

        public async Task<AnswerFromVectorDto> QueryByVector(SearchEmbeddingDto searchEmbedding, AppSettings appSettings)
        {
            AnswerFromVectorDto answerFromVectorDto = new AnswerFromVectorDto();

            //Create search string embedding
            var searchStringEmbedding = await CreateEmbedding(appSettings, searchEmbedding.EmbeddingLanguageModel, searchEmbedding.AskedOrSearch);
            if (!searchStringEmbedding.IsSuccessful)
                return answerFromVectorDto;

            //Search by vector
            var createdEmbeddingsResponseDto = (CreatedEmbeddingsResponseDto)searchStringEmbedding.Data;
            var searchStringVector = createdEmbeddingsResponseDto.Data.FirstOrDefault().Embedding;
            var scoredVectors = await _pineConeService.QueryByVector(searchEmbedding, searchStringVector, appSettings);
            if (!scoredVectors.IsSuccessful)
                return answerFromVectorDto;

            string filterredString = "";
            List<int> noOfPages = new List<int>();
            foreach ( var scoredVector in (ScoredVector[])scoredVectors.Data)
            {
                filterredString += scoredVector.Metadata.FirstOrDefault().Value.Inner;
                noOfPages.Add(Convert.ToInt16(scoredVector.Metadata.ToList()[2].Value.Inner));
            }

            //Get search result from filtered context which is get by vector search.
            CompletionsRequestDto completionsRequestDto = new CompletionsRequestDto();
            completionsRequestDto.Model = searchEmbedding.GPTLanguageModel;// "text-davinci-003";
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

            var result  = await _completionsService.Completions(completionsRequestDto, appSettings.OpenAiAPIkey, appSettings.OpenAiBaseUrl);
            
            if (!result.IsSuccessful)
                return answerFromVectorDto;

            answerFromVectorDto.IsSuccessful = true;
            answerFromVectorDto.CreatedEmbeddingsResponse = createdEmbeddingsResponseDto;
            answerFromVectorDto.CompletionsResponse = (CompletionsResponseDto)result.Data;           
            answerFromVectorDto.NoOfPages = noOfPages.Distinct().ToList();

            return answerFromVectorDto;

        }

    }
}
