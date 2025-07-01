using SurveyBasket.API.Abstractions;
using SurveyBasket.API.DtoRequestAndResponse.Result;

namespace SurveyBasket.API.Repositories
{
    public interface IResultVote
    {
        public Task<Result<PollVotesResponse>> GetVoteResultAsync(int pollId , CancellationToken cancellationToken = default);
        public Task<Result<IEnumerable<VotePerDayResponse>>> GetVotePerDayAsync(int pollId, CancellationToken cancellationToken = default);
        public Task<Result<IEnumerable<VotePerQuestionResponse>>> GetVotePerQuestionAsync(int pollId, CancellationToken cancellationToken = default);

    }
}
