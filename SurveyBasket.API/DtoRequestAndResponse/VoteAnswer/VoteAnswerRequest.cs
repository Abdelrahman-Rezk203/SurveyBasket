namespace SurveyBasket.API.DtoRequestAndResponse.VoteAnswer
{
    public record VoteAnswerRequest(
        int QuestionId,
        int AnswerId
        );
    
}
