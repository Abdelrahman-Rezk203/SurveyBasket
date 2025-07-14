using SurveyBasket.API.Abstractions.ResultPattern;
using SurveyBasket.API.DtoRequestAndResponse.Vote;

namespace SurveyBasket.API.Repositories
{
    public interface IVoteSevice
    {
        public Task<Result> AddVoteAsync(int PollId, string UserId, VoteRequest voteRequest, CancellationToken cancellationToken = default);
    }
}
