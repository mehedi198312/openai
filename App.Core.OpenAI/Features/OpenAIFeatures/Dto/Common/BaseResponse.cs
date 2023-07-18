namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Common
{
    public class BaseResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
