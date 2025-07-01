using SurveyBasket.API.Entities;

namespace SurveyBasket.API.DtoRequestAndResponse.Result
{
    public record VotePerQuestionResponse(
        string Question, 
        IEnumerable<VotePerAnswerResponse> SelectAnswers
        );
    
}
