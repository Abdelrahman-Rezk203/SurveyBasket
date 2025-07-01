namespace SurveyBasket.API.DtoRequestAndResponse.Result
{
    public record VoteResponse(
        string FullName,
        IEnumerable<QuestionAnswerResponse> SelectAnswers
        );
    
}
