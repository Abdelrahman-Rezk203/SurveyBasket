namespace SurveyBasket.API.DtoRequestAndResponse.Result
{
    public record VotePerDayResponse(
        DateOnly Date,
        int NumOfVotes
        );
   
}
