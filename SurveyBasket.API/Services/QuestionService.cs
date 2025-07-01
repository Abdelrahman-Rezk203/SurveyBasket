using Mapster;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.DtoRequestAndResponse.Answer;
using SurveyBasket.API.DtoRequestAndResponse.Question;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Persistance.DbContext;
using SurveyBasket.API.Repositories;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;

namespace SurveyBasket.API.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly HybridCache _hybridCache;
        private readonly ICacheService _cacheService;
        private readonly ILogger<QuestionService> _logger;

        public QuestionService(ApplicationDbContext applicationDbContext, HybridCache hybridCache , ICacheService cacheService, ILogger<QuestionService> logger)
        {
            _applicationDbContext = applicationDbContext;
            _hybridCache = hybridCache;
            _cacheService = cacheService;
            _logger = logger;
        }

        private const string _CachePrefix = "availableQuestion";

        //عشان اشوف الاول هو موجود ولا لا مينفعش اضيف غير لما يكون موجود عشان اربط العلاقات ببعض  PollId اللويجك هيتغير لان الجدول بتاع الاسئله مرتبط مع ال البول يعني لازم تبعتلي 
        public async Task<Result<QuestionResponse>> AddQuestionAsync(int PollId, QuestionRequest questionRequest, CancellationToken cancellationToken = default)
        {
            var PollIsFound = await _applicationDbContext.Polls.FindAsync(PollId,cancellationToken);
            if (PollIsFound is null)
                return Result.Failure<QuestionResponse>(PollErrors.PollIdNotFound);
            
            ///var QuestionEntity = new Question()  // دا هيبقي غلط عشان انا رايبط قبل متعمل تحويل تحت يعني لما يحول هيشيله تاني 
            ///{
            ///   PollID = PollIsFound.Id    //Connect Qusetion With Poll by FK
            ///};
            
            if (string.IsNullOrWhiteSpace(questionRequest.Content))
                return Result.Failure<QuestionResponse>(QuestionError.ContentNotEmpty);

            var QuestionDublicated = await _applicationDbContext.Questions.AnyAsync(x => x.Content == questionRequest.Content && x.PollID == PollId);
            if (QuestionDublicated)                                                                                         //x.PollID == PollId  عشان اضيف ف استطلاع راي تاني عادي مينفعش امنع خالص
                return Result.Failure<QuestionResponse>(QuestionError.ContentDublicated);

            var ConvertToQuestion = questionRequest.Adapt<Question>(); 

              ConvertToQuestion.PollID = PollId; //Connect Qusetion With Poll by FK

            // MappingConfig فيه جزء مهم خاص بالانسر في 

            await _applicationDbContext.AddAsync(ConvertToQuestion, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            await _hybridCache.RemoveAsync($"{ _CachePrefix}-{PollId}",cancellationToken);

            return Result.Success(ConvertToQuestion.Adapt<QuestionResponse>());



        }

        public async Task<Result<QuestionWithOutAnswerResponse>> AddToggleQuestionsStatusAsync(int PollId, int Id, CancellationToken cancellationToken = default)
        {
            var CheckPollIsFound = await _applicationDbContext.Polls.FindAsync(PollId , cancellationToken);
            if (CheckPollIsFound is null)
                return Result.Failure<QuestionWithOutAnswerResponse>(PollErrors.PollIdNotFound);
                                                                                            //انا بدور ف كل جدول الكويسشن فلازم اقوله متعدلش غير علي اللي موجود جوه البول دي فقط
            var CheckQuestionIsFound = await _applicationDbContext.Questions.FirstOrDefaultAsync(x=>x.Id == Id && x.PollID == CheckPollIsFound.Id , cancellationToken);
            if (CheckQuestionIsFound is null )
                return Result.Failure<QuestionWithOutAnswerResponse>(QuestionError.QuestionNotFound);

            CheckQuestionIsFound.IsActive = !CheckQuestionIsFound.IsActive;
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            //var request = new QuestionResponse() 
            //{
            //    Id = CheckQuestionIsFound.Id,
            //    Content = CheckQuestionIsFound.Content

            //};
            await _hybridCache.RemoveAsync($"{_CachePrefix}-{PollId}", cancellationToken);

            return Result.Success(CheckQuestionIsFound.Adapt<QuestionWithOutAnswerResponse>());
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> GetAllQuestionsAsync(int PollId , CancellationToken cancellationToken = default)
        {
            var GetQuestioninSpecificPoll = await _applicationDbContext.Polls.FindAsync(PollId, cancellationToken);
            if (GetQuestioninSpecificPoll is null)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);

               var AllQuestion = await _applicationDbContext.Questions
                                                        .AsNoTracking()
                                                        .Where(x=>x.PollID == PollId)
                                                        .Include(x=>x.Answers)
                                                        .ToListAsync(cancellationToken);

            return Result.Success(AllQuestion.Adapt<IEnumerable<QuestionResponse>>());
        }

        public async Task<Result<QuestionResponse>> GetQuestionsByIdAsync(int PollId , int Id, CancellationToken cancellationToken = default)
        {
            var CheckPollExist = await _applicationDbContext.Polls.FindAsync(PollId, cancellationToken);
            if (CheckPollExist is null)
                return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

            var CheckQuestionExist = await _applicationDbContext.Questions.AnyAsync(x => x.Id == Id &&  x.PollID == CheckPollExist.Id , cancellationToken);
            if (!CheckQuestionExist )
                return Result.Failure<QuestionResponse>(QuestionError.QuestionNotFound);

            var GetQuestion = await _applicationDbContext.Questions.AsNoTracking()        //ممكن البول موجوده لكن مفهاش الكويسشن ده 
                                                                   .Where(x => x.PollID == PollId && x.Id == Id) //كذا  Id عشان اتاكد ان البول موجوده بالفعل وكمان فيها كويسشن عنده 
                                                                   .Include(x => x.Answers)
                                                                   .FirstOrDefaultAsync(cancellationToken);
                                                                 //.ToListAsync(cancellationToken); //List<QuestionResponse> مش هحول الي ليست عشان هو واحد فقط بس لو حولت لازم ارجع          
            
            return Result.Success(GetQuestion.Adapt<QuestionResponse>());
        }

        //public async Task<Result> UpdateQuestion(int PollId, int Id, QuestionRequest questionRequest , CancellationToken cancellationToken = default)
        //{
        //    var CheckPollIsFound = await _applicationDbContext.Polls.AnyAsync(x => x.Id == PollId, cancellationToken);
        //    if (!CheckPollIsFound)
        //        return Result.Failure(PollErrors.PollIdNotFound);
        //                                                                   //عشان يعرف ان فيه اجابات مرتبطه مع السوال فلما يمسح يروح يمسحها
        //    var CheckQuestionIsFound = await _applicationDbContext.Questions.Include(x=>x.Answers).FirstOrDefaultAsync(x=>x.Id == Id && x.PollID == PollId , cancellationToken);
        //    if (CheckQuestionIsFound is null)
        //        return Result.Failure(QuestionError.QuestionNotFound);
        //                                                       //عشان ميقارنش نفسه بنفسه اللي هو بنعدل عليه حاليا 
        //    var IsDublicated = await _applicationDbContext.Questions.AnyAsync(x=>x.Id != Id && x.PollID == PollId && x.Content == questionRequest.Content,cancellationToken);
        //    if (IsDublicated)                                               //مش اللي انا واقف عنده 
        //        return Result.Failure(QuestionError.ContentDublicated);


        //    CheckQuestionIsFound.Content = questionRequest.Content;

        //    _applicationDbContext.Answers.RemoveRange(CheckQuestionIsFound.Answers); // delete old Answers 
        //                                                                            //الكونتنت اللي ف الانسر هو الاجابات 
        //    //CheckQuestionIsFound.Answers = questionRequest.Answers.Select(x => new Answer { Content = x}).ToList();
        //    foreach (var item in questionRequest.Answers)
        //    {
        //        var answer = new Answer
        //        {
        //            Content =  item,
        //            QuestionId = CheckQuestionIsFound.Id
        //        };
        //        await _applicationDbContext.Answers.AddAsync(answer);
        //    }

        //    await _applicationDbContext.SaveChangesAsync(cancellationToken);
        //    return Result.Success();


        //}

        public async Task<Result> UpdateQuestion(int PollId, int Id, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var CheckPollIsFound = await _applicationDbContext.Polls.AnyAsync(x => x.Id == PollId, cancellationToken);
            if (!CheckPollIsFound)
                return Result.Failure(PollErrors.PollIdNotFound);
                                                                                   //عشان يعرف ان فيه اجابات مرتبطه مع السوال فلما يمسح يروح يمسحها
            var question = await _applicationDbContext.Questions.Include(x => x.Answers).FirstOrDefaultAsync(x => x.Id == Id && x.PollID == PollId, cancellationToken);
            if (question is null)
                return Result.Failure(QuestionError.QuestionNotFound);
                                                              //عشان ميقارنش نفسه بنفسه اللي هو بنعدل عليه حاليا 
            var IsDublicated = await _applicationDbContext.Questions.AnyAsync(x => x.Id != Id && x.PollID == PollId && x.Content == request.Content, cancellationToken);
            if (IsDublicated)                                               //مش اللي انا واقف عنده 
                return Result.Failure(QuestionError.ContentDublicated);


            question.Content = request.Content;

            var CurrentAnswer = question.Answers.Select(x => x.Content).ToList(); //get current answer

            var NewAnswer = request.Answers.Except(CurrentAnswer).ToList(); //get new answer

            NewAnswer.ForEach(x =>  //add new answer in DataBase
            {
                question.Answers.Add(new Answer { Content = x });
            });

            question.Answers.ToList().ForEach(x => // هلف علي كل الداتا بيز عشان اشوف مين هيتاكتف ومين لا
            {
                x.IsActive = request.Answers.Contains(x.Content);
                //هل الانسر اللي جايه موجوده ف الداتا بيز ولا لا لو رجع ترو هييفعلها لو لا هيحط فالس 
            });
            await _hybridCache.RemoveAsync($"{_CachePrefix}-{PollId}",cancellationToken);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        // If the user has already voted for this specific poll,
        // then don't return the questions because they are not allowed to vote twice. //طاملا عملت فوت يبقي معندش اي اسئله متاحه في نفس البول اللي عملت فوت عليها 
        public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailableQuestionToVoteAsync(int PollId, string UserId, CancellationToken cancellationToken = default)
        {
            var CheckPollIsFound = await _applicationDbContext.Polls.AnyAsync(x=>x.Id == PollId && x.IsPublisher &&
                                                                    DateOnly.FromDateTime(DateTime.UtcNow) >= x.StartsAt 
                                                                    && DateOnly.FromDateTime(DateTime.UtcNow)<= x.EndsAt , cancellationToken);
            if (!CheckPollIsFound)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFoundOrRevoked);
           
            var hasVote = await _applicationDbContext.Votes.AnyAsync(x => x.UserId == UserId && x.PollId == PollId, cancellationToken);
            if (hasVote)
                return Result.Failure<IEnumerable<QuestionResponse>>(VoteError.DublicatedVote);


            var cacheKey = $"{_CachePrefix}-{PollId}"; //id وعندها نفس ال  availableQuestion  كون اسمها  

            var CacheQuestion = await _hybridCache.GetOrCreateAsync<IEnumerable<QuestionResponse>>(
                cacheKey,
                //_logger.LogInformation("Select From Database");
                async cacheEntry =>
                    {
                        return await _applicationDbContext.Questions
                                                                   .Where(x => x.IsActive && x.PollID == PollId)
                                                                   .Include(x => x.Answers)
                                                                   .Select(q => new QuestionResponse
                                                                       (
                                                                       q.Id,
                                                                       q.Content,
                                                                       q.Answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.Id, a.Content)).ToArray()
                                                                       )//هحددد الاجابات اللي اكتف 
                                                                   ).AsNoTracking()
                                                                   .ToListAsync(cancellationToken);

                    });
           
            return Result.Success(CacheQuestion);
        }

        public async Task<Result> AddToggleAnswersStatusAsync(int pollId , int QuestionId , int AnswerId,CancellationToken  cancellationToken = default)
        {
            var PollIIsFound = await _applicationDbContext.Polls.AnyAsync(x => x.IsPublisher &&
                                                                          x.Id == pollId &&
                                                                          DateOnly.FromDateTime(DateTime.UtcNow) <= x.EndsAt &&
                                                                          DateOnly.FromDateTime(DateTime.UtcNow) >= x.StartsAt
                                                                          , cancellationToken);
            if(!PollIIsFound)
                return Result.Failure(PollErrors.PollNotFound);

            var QuestionIsFound = await _applicationDbContext.Questions.AnyAsync(x => x.PollID == pollId &&
                                                                                 x.IsActive &&
                                                                                 x.Id == QuestionId
                                                                                 , cancellationToken);
            if(!QuestionIsFound)
                return Result.Failure(QuestionError.QuestionNotFoundOrNotActive);


            var AnswerIsFound = await _applicationDbContext.Answers.FirstOrDefaultAsync(x => x.Id == AnswerId &&
                                                                                        x.QuestionId == QuestionId
                                                                                        , cancellationToken);

            if(AnswerIsFound is null)
                return Result.Failure(AnswerError.AnswerNotFound);

            AnswerIsFound.IsActive = !AnswerIsFound.IsActive ;
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }




    }
}
