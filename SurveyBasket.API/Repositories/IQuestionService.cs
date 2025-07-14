using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Abstractions.ResultPattern;
using SurveyBasket.API.DtoRequestAndResponse.Common;
using SurveyBasket.API.DtoRequestAndResponse.Question;

namespace SurveyBasket.API.Repositories
{
    public interface IQuestionService
    {
        public Task<Result<IEnumerable<QuestionResponse>>> GetAvailableQuestionToVoteAsync(int PollId, string UserId, CancellationToken cancellationToken = default);
        public Task<Result<QuestionResponse>>AddQuestionAsync(int PollId, QuestionRequest questionRequest, CancellationToken cancellationToken = default);
        public Task<Result<PaginationList<QuestionResponse>>> GetAllQuestionsAsync(int PollId, RequestFilter requestFilter, CancellationToken cancellationToken = default);
        public Task<Result<QuestionResponse>>GetQuestionsByIdAsync(int PollId , int Id ,CancellationToken cancellationToken = default);
        public Task<Result<QuestionWithOutAnswerResponse>>AddToggleQuestionsStatusAsync(int PollId , int Id ,CancellationToken cancellationToken = default);
        public Task<Result> AddToggleAnswersStatusAsync(int pollId, int QuestionId, int AnswerId, CancellationToken cancellationToken = default);
        public Task<Result> UpdateQuestion(int PollId, int Id, QuestionRequest questionRequest , CancellationToken cancellationToken = default);

    }
}
