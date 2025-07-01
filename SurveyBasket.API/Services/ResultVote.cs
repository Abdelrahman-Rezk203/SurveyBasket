using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.DtoRequestAndResponse.Result;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Persistance.DbContext;
using SurveyBasket.API.Repositories;
using System.Collections.Generic;

namespace SurveyBasket.API.Services
{
    public class ResultVote : IResultVote
    {
        private readonly ApplicationDbContext _context;

        public ResultVote(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<VotePerDayResponse>>> GetVotePerDayAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var PollIsExist = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);
            if (!PollIsExist)
                return Result.Failure<IEnumerable<VotePerDayResponse>>(PollErrors.PollIdNotFound);

            var VotePerDay = await _context.Votes
                                                 .Where(x => x.Id == pollId)
                                                 .GroupBy(g => new { Date = DateOnly.FromDateTime(g.SubmittedOn) })
                                                 .Select(g => new VotePerDayResponse(
                                                     g.Key.Date,
                                                     g.Count()
                                                 )).ToListAsync(cancellationToken);

            if (VotePerDay is null)
                return Result.Failure<IEnumerable<VotePerDayResponse>>(PollErrors.PollIdNotFound);

            return Result.Success<IEnumerable<VotePerDayResponse>>(VotePerDay);

        }

        public async Task<Result<IEnumerable<VotePerQuestionResponse>>> GetVotePerQuestionAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var PollIsExist = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);
            if (!PollIsExist)
                return Result.Failure<IEnumerable<VotePerQuestionResponse>>(PollErrors.PollNotFound);

            var VotePerQuestion = await _context.VoteAnswers
                                                            .Where(x => x.Vote.PollId == pollId)
                                                            .Select(x => new VotePerQuestionResponse(
                                                                x.Question.Content,
                                                                x.Question.VoteAnswers.GroupBy(x => new { AnswerId = x.Answer.Id, AnswerContent = x.Answer.Content })
                                                                .Select(x => new VotePerAnswerResponse(
                                                                    x.Key.AnswerContent,
                                                                    x.Count()
                                                                    ))
                                                                )).ToListAsync(cancellationToken);

            if (VotePerQuestion is null)
                return Result.Failure<IEnumerable<VotePerQuestionResponse>>(PollErrors.PollIdNotFound);

            return Result.Success<IEnumerable<VotePerQuestionResponse>>(VotePerQuestion);
        }

        public async Task<Result<PollVotesResponse>> GetVoteResultAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var PollIsExist = await _context.Polls.AnyAsync(x=>x.Id == pollId , cancellationToken); 
            if (!PollIsExist) 
                return Result.Failure<PollVotesResponse>(PollErrors.PollNotFound);

            var PollVote = await _context.Polls
                                         .Where(x => x.Id == pollId)
                                         .Select(x => new PollVotesResponse(
                                             x.Title,
                                             x.StartsAt,
                                             x.EndsAt,
                                             x.votes.Select(x => new VoteResponse(
                                                 $"{x.User.FirstName} {x.User.LastName}",
                                                 x.VoteAnswers.Select(x => new QuestionAnswerResponse(
                                                     x.Question.Content,
                                                     x.Answer.Content
                                                     ))
                                                 ))
                                             )).FirstOrDefaultAsync(cancellationToken);
            if (PollVote is null)
                return Result.Failure<PollVotesResponse>(PollErrors.PollNotFound);

            return Result.Success(PollVote);
        }




    }
}
