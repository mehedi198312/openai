namespace App.Core.OpenAI.Features.OpenAIFeatures.Dto.Completions
{
    public class QuestionSetDto
    {
        public bool IsSuccessful { get; set; }
        public CompletionsResponseDto QuestionSet { get; set; }
    }
}
