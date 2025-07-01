using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.DtoRequestAndResponse.Vote;
using SurveyBasket.API.DtoRequestAndResponse.VoteAnswer;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Persistance.DbContext;
using SurveyBasket.API.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace SurveyBasket.API.Services
{
    public class VoteService : IVoteSevice
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public VoteService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Result> AddVoteAsync(int PollId, string UserId, VoteRequest voteRequest, CancellationToken cancellationToken = default)
        {
            var PollIsFound = await _applicationDbContext.Polls.AnyAsync(x => x.Id == PollId &&
                                                                       x.IsPublisher &&
                                                                       x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow) &&
                                                                       x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
                                                                       , cancellationToken);
            if (!PollIsFound)
                return Result.Failure(PollErrors.PollIdNotFound);

            var UserIsVoted = await _applicationDbContext.Votes.AnyAsync(x => x.UserId == UserId && x.PollId == PollId, cancellationToken);
            if (UserIsVoted)
                return Result.Failure(VoteError.DublicatedVote);

            var AvailableVote = await _applicationDbContext.Questions // عشان البول فيه اساله كتير اراي فنا عايز اشوف كل اللي جواها Where  هستخدم ال
                                    .Where(x => x.IsActive && x.PollID == PollId) //اللي جوه الكويسشن Id هفلتر ال 
                                    .Select(x => x.Id) //id هعملها سيليكت لل   
                                    .ToListAsync(cancellationToken);

            //اللي بتاع الكويسشن اللي متاح اني اعمل عليها فوت  id هرجع ال
            if (!voteRequest.Answers.Select(x => x.QuestionId).All(x => AvailableVote.Contains(x))) //AvailableVote اللي جايه ف الريكويست تكون زي ال  id لازم ال 
                return Result.Failure(VoteError.InvalidQuestion); // اللي جايه ف الريكويست تكون من ضمن المتاحين اعمل عليهم فوت id لازم ال  
            // All مش لازم يجاوب علي كل الاسئله
            //Sequential لازم يجاوب علي كل الاسئله بالترتيب

            var AnswerIdIsExist = await _applicationDbContext.Answers
                                                             .Where(x=>x.IsActive)
                                                             .Select(x => x.Id)
                                                             .ToListAsync(cancellationToken);

            foreach (var item in voteRequest.Answers)
            {
                if (!AnswerIdIsExist.Contains(item.AnswerId))
                    return Result.Failure(VoteError.InvalidAnswers);
            }


            var vote = new Vote
            {
                PollId = PollId,
                UserId = UserId,
                VoteAnswers = voteRequest.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList() //entity هحوله لل 
            };
            await _applicationDbContext.AddAsync(vote,cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
    }
    
    }
