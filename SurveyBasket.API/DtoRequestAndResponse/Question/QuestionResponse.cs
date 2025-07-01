using SurveyBasket.API.DtoRequestAndResponse.Answer;

namespace SurveyBasket.API.DtoRequestAndResponse.Question
{
    public record QuestionResponse(
        int Id,
        string Content, 
        IEnumerable<AnswerResponse> Answers
        );
   
}
