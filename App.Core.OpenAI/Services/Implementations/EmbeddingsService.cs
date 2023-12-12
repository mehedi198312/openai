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
using System.IO;
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

        private async Task<QuestionSetDto> CreateQuestionSet(string path, string languageModel, AppSettings appSettings)
        {
            QuestionSetDto sampleQuestionsSet = new QuestionSetDto();

            string context = "";

            PdfReader reader = new PdfReader(path);
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                context = context + " " + PdfTextExtractor.GetTextFromPage(reader, page);
            }
            reader.Close();

            context = context.Replace('\n', ' ');

            //Take context for preparing sample question set 
            if (context.Length > appSettings.ContextLengthForQuestionSet)
                context = context.Substring(0, appSettings.ContextLengthForQuestionSet);


            //Get a set of sample questions which are highly depends on provided file.
            CompletionsRequestDto completionsRequestDto = new CompletionsRequestDto();
            completionsRequestDto.Model = languageModel;// "text-davinci-003";
            completionsRequestDto.Temperature = appSettings.Temperature;//  0;
            completionsRequestDto.MaxTokens = appSettings.MaxTokens;//  150;
            completionsRequestDto.TopP = appSettings.TopP;//  1;
            completionsRequestDto.FrequencyPenalty = appSettings.FrequencyPenalty;//  0;
            completionsRequestDto.PresencePenalty = appSettings.PresencePenalty;//  0;

            completionsRequestDto.Prompt = "Create a question set based on the context below, " +
                "and if unable to create a set of question based on the context, say \"Sorry, I am unable to generate question set.\"\n\n" +
                "Context: " + context + "\n\n---\n\n";

            var result = await _completionsService.Completions(completionsRequestDto, appSettings.OpenAiAPIkey, appSettings.OpenAiBaseUrl);

            #region "Chat Model unable to generate question set 11/12/2023"

            //ChatCompletionsRequestDto chatCompletionsWithFileRequestDto = new ChatCompletionsRequestDto();
            //chatCompletionsWithFileRequestDto.Model = languageModel;
            //chatCompletionsWithFileRequestDto.Temperature = appSettings.Temperature;
            //chatCompletionsWithFileRequestDto.MaxTokens = appSettings.MaxTokens;
            //chatCompletionsWithFileRequestDto.TopP = appSettings.TopP;
            //chatCompletionsWithFileRequestDto.FrequencyPenalty = appSettings.FrequencyPenalty;
            //chatCompletionsWithFileRequestDto.PresencePenalty = appSettings.PresencePenalty;
            //chatCompletionsWithFileRequestDto.Messages = new List<ChatCompletionsMessagesRequestDto>();

            //chatCompletionsWithFileRequestDto.Messages.Add(
            //    new ChatCompletionsMessagesRequestDto
            //    {
            //        Role = "system",
            //        Content = "Create a question set based on the context below, " +
            //            "and if unable to create a set of question based on the context, say \"Sorry, I am unable to generate question set.\"\n\n"
            //    });

            //chatCompletionsWithFileRequestDto.Messages.Add(
            //    new ChatCompletionsMessagesRequestDto
            //    {
            //        Role = "user",
            //        Content = "Context: " + context + "\n\n---\n\n"+
            //        "Question:"
            //    });

            //var result = await _completionsService.ChatCompletions(chatCompletionsWithFileRequestDto, appSettings.OpenAiAPIkey, appSettings.OpenAiBaseUrl);


            #endregion

            if (!result.IsSuccessful)
                return sampleQuestionsSet;

            sampleQuestionsSet.IsSuccessful = true;            
            sampleQuestionsSet.QuestionSet = (CompletionsResponseDto)result.Data;

            return sampleQuestionsSet;

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

            //Create chunk from file context file
            var createChunkReponse = await CreateChunk(fileUploadResponse.Data.ToString(), appSettings.ChunkSize, appSettings.ChunkOverlap);
            if(!createChunkReponse.IsSuccessful)
                generatedEmbeddingsDto.Message = createChunkReponse.Message;

            //Generate a quesiton set based on provided file context
            var questionSetResponse = await CreateQuestionSet(fileUploadResponse.Data.ToString(), appSettings.Model, appSettings);

            //Generate embedding/vector using OpenAI embedding API
            var createEmbeddingsResponse = await CreateEmbeddings(appSettings, embeddingsFile.EmbeddingLanguageModel, ((List<ChunkDto>)createChunkReponse.Data));
            if (createEmbeddingsResponse.IsSuccessful)
            {
                //Save vector into pinecone vector database
                var createdEmbeddingsResponseDto = (CreatedEmbeddingsResponseDto)createEmbeddingsResponse.Data;
                await _pineConeService.UpsertList(createdEmbeddingsResponseDto.Data, ((List<ChunkDto>)createChunkReponse.Data), embeddingsFile, appSettings);
            
                //return response
                generatedEmbeddingsDto.CreatedEmbeddingsResponse = createdEmbeddingsResponseDto;
                generatedEmbeddingsDto.QuestionSet = questionSetResponse.QuestionSet;
                generatedEmbeddingsDto.IsSuccessful = true;
            }

            return generatedEmbeddingsDto;
        }

        public async Task<AnswerFromVectorDto> QueryByVector(ChatCompletionsWithFileRequestDto chatCompletionsWithFileRequestDto, AppSettings appSettings)
        {
            AnswerFromVectorDto answerFromVectorDto = new AnswerFromVectorDto();
            var questionOfUser = chatCompletionsWithFileRequestDto.AskedOrSearch;

            //Create search string embedding
            var searchStringEmbedding = await CreateEmbedding(appSettings, chatCompletionsWithFileRequestDto.EmbeddingLanguageModel, questionOfUser);
            if (!searchStringEmbedding.IsSuccessful)
                return answerFromVectorDto;

            //Search by vector
            var createdEmbeddingsResponseDto = (CreatedEmbeddingsResponseDto)searchStringEmbedding.Data;
            var searchStringVector = createdEmbeddingsResponseDto.Data.FirstOrDefault().Embedding;
            var scoredVectors = await _pineConeService.QueryByVector(chatCompletionsWithFileRequestDto, searchStringVector, appSettings);
            if (!scoredVectors.IsSuccessful)
                return answerFromVectorDto;

            string filterredString = "";
            List<int> noOfPages = new List<int>();
            foreach ( var scoredVector in (ScoredVector[])scoredVectors.Data)
            {
                filterredString += scoredVector.Metadata.FirstOrDefault().Value.Inner;
                noOfPages.Add(Convert.ToInt16(scoredVector.Metadata.ToList()[2].Value.Inner));
            }
           
            ChatCompletionsRequestDto chatCompletionsRequestDto = new ChatCompletionsRequestDto();
            chatCompletionsRequestDto.Model = chatCompletionsWithFileRequestDto.GPTModel;
            chatCompletionsRequestDto.Temperature = chatCompletionsWithFileRequestDto.Temperature;
            chatCompletionsRequestDto.MaxTokens = chatCompletionsWithFileRequestDto.MaxTokens;
            chatCompletionsRequestDto.TopP = chatCompletionsWithFileRequestDto.TopP;
            chatCompletionsRequestDto.FrequencyPenalty = chatCompletionsWithFileRequestDto.FrequencyPenalty;
            chatCompletionsRequestDto.PresencePenalty = chatCompletionsWithFileRequestDto.PresencePenalty;
            chatCompletionsRequestDto.Messages = new List<ChatCompletionsMessagesRequestDto>();

            chatCompletionsRequestDto.Messages.Add(
                new ChatCompletionsMessagesRequestDto
                {
                    Role = "system",
                    Content = "Answer the question based on the context below, " +
                        "and if the question can't be answered based on the context, say \"I don't know\"\n\n"
                });

            chatCompletionsRequestDto.Messages.Add(
                new ChatCompletionsMessagesRequestDto
                {
                    Role = "user",
                    Content = "Context: " + filterredString + "\n\n---\n\n" +
                              "Question: " + chatCompletionsWithFileRequestDto.AskedOrSearch + "\n" +
                              "Answer:"
                });

            var result  = await _completionsService.ChatCompletions(chatCompletionsRequestDto, appSettings.OpenAiAPIkey, appSettings.OpenAiBaseUrl);
            
            if (!result.IsSuccessful)
                return answerFromVectorDto;

            answerFromVectorDto.IsSuccessful = true;
            answerFromVectorDto.CreatedEmbeddingsResponse = createdEmbeddingsResponseDto;
            answerFromVectorDto.CompletionsResponse = (ChatCompletionsResponseDto)result.Data;           
            answerFromVectorDto.NoOfPages = noOfPages.Distinct().ToList();

            return answerFromVectorDto;

        }

    }
}
