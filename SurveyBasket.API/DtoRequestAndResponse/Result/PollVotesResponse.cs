namespace SurveyBasket.API.DtoRequestAndResponse.Result
{
    public record PollVotesResponse(
        string Title,
        DateOnly StartAt,
        DateOnly EndAt,
        IEnumerable<VoteResponse> Votes
        );
    
}
