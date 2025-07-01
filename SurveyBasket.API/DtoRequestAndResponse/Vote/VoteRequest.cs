using SurveyBasket.API.DtoRequestAndResponse.Answer;
using SurveyBasket.API.DtoRequestAndResponse.VoteAnswer;

namespace SurveyBasket.API.DtoRequestAndResponse.Vote
{
    public record VoteRequest(
         IEnumerable<VoteAnswerRequest> Answers
        );
    
}
