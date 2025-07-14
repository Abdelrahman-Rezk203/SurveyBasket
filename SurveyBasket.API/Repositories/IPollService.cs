using SurveyBasket.API.Abstractions.ResultPattern;
using SurveyBasket.API.Dto.Polls;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Repositories
{
    public interface IPollService
    {
        public Task<Result<IEnumerable<PollResponse>>> GetCurrentPollAsync(CancellationToken cancellationToken = default);
        public Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<Result<PollResponse>> GetPollByIdAsync(int? Id, CancellationToken cancellationToken = default);
        public Task<Result<PollResponse>> AddPollAsync(PollDtoRequest pollDto, CancellationToken cancellationToken = default);
        public Task<Result> UpdatePollAsync(int Id, PollDtoRequest pollDto, CancellationToken cancellationToken = default);
        public Task<Result> DeletePollAsync(int Id, CancellationToken cancellationToken = default);
        public Task<Result> TogglePublishStatusAsync(int Id, CancellationToken cancellationToken = default);

    }
}
