using System.Text.Json.Serialization;

namespace SurveyBasket.API.DtoRequestAndResponse.Question
{
    public record QuestionRequest(
        string Content,
        List<string> Answers
        );
    
}
