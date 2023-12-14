using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Audio;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common;
using App.Core.OpenAI.Features.OpenAIFeatures.Dto.Error;
using App.Core.OpenAI.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace App.Core.OpenAI.Services.Implementations
{
    public class AudioService : IAudioService
    {

        private readonly IConfiguration _configuration;

        public AudioService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse> CreateTranscriptions(CreateTranscriptionsRequestDto request, string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();
            var multipartContent = new MultipartFormDataContent();

            byte[] audioFile;
            using (var ms = new MemoryStream())
            {
                request.File.CopyTo(ms);
                audioFile = ms.ToArray();
            }
            multipartContent.Add(new ByteArrayContent(audioFile), "file", request.File.FileName);
            multipartContent.Add(new StringContent(request.Model), "model");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"{baseUrl}/audio/transcriptions", multipartContent);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }

            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<CreateTranscriptionsResponseDto>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> CreateTranslations(CreateTranslationsRequestDto request, string token, string baseUrl)
        {
            var baseResponse = new BaseResponse();
            var multipartContent = new MultipartFormDataContent();

            byte[] audioFile;
            using (var ms = new MemoryStream())
            {
                request.File.CopyTo(ms);
                audioFile = ms.ToArray();
            }
            multipartContent.Add(new ByteArrayContent(audioFile), "file", request.File.FileName);
            multipartContent.Add(new StringContent(request.Model), "model");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"{baseUrl}/audio/translations", multipartContent);
            var resjson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                baseResponse.IsSuccessful = false;
                baseResponse.Data = errorResponse;
            }

            baseResponse.IsSuccessful = true;
            baseResponse.Data = JsonSerializer.Deserialize<CreateTranslationsResponseDto>(resjson);
            return baseResponse;
        }

        public async Task<BaseResponse> TextToSpeech(TextToSpeechRequestDto request, AppSettings appSettings)
        {
            var baseResponse = new BaseResponse();
            byte[] audioFile = new byte[1];

            #region "Chunk create App end"

            //var textToSpeechAPIResponse = await CallTextToSpeechAPI(request, appSettings);
            //if (textToSpeechAPIResponse.IsSuccessful)
            //{
            //    audioFile = (byte[])textToSpeechAPIResponse.Data;
            //}

            #endregion

            #region "Chunk create here"

            int chunkSize = Convert.ToInt16(_configuration.GetSection("Audio").GetSection("ChunkSize").Value);
            List<string> chunks = request.Input.Chunk(chunkSize).Select(x => new string(x)).ToList();

            //Generate audio for first chunk
            request.Input = chunks.FirstOrDefault();
            var generateAudioForFirstChunk = await CallTextToSpeechAPI(request, appSettings);
            if (generateAudioForFirstChunk.IsSuccessful)
            {
                audioFile = (byte[])generateAudioForFirstChunk.Data;
            }

            //Generate audio for rest other chunks
            var restChunks = chunks.Skip(1).ToList();
            foreach (var chunk in restChunks)
            {
                request.Input = chunk;
                var generateAudio = await CallTextToSpeechAPI(request, appSettings);

                if (generateAudio.IsSuccessful)
                {
                    var newBytes = (byte[])generateAudio.Data;

                    byte[] newArray = new byte[audioFile.Length + newBytes.Length];
                    audioFile.CopyTo(newArray, 0);
                    for (int i = 0; i < newBytes.Length; i++)
                    {
                        newArray[audioFile.Length + i] = newBytes[i];
                    }
                    audioFile = newArray;
                }
            }

            #endregion

            //var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), appSettings.TTLFile, $"{Guid.NewGuid()}.mp3");      
            //File.WriteAllBytes(path, speechInBytes);

            baseResponse.IsSuccessful = true;
            baseResponse.Data = audioFile;
            return baseResponse;
        }

        private async Task<BaseResponse> CallTextToSpeechAPI(TextToSpeechRequestDto request, AppSettings appSettings)
        {
            var baseResponse = new BaseResponse();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.OpenAiAPIkey);

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{appSettings.OpenAiBaseUrl}/audio/speech", content);
            var speechInBytes = await response.Content.ReadAsByteArrayAsync();
        
            baseResponse.IsSuccessful = true;
            baseResponse.Data = speechInBytes;
            return baseResponse;
        }

    }
}
