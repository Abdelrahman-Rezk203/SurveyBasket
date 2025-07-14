using Hangfire;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions.ResultPattern;
using SurveyBasket.API.Dto.Polls;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Models;
using SurveyBasket.API.Persistance.DbContext;
using SurveyBasket.API.Repositories;
using System.Data;

namespace SurveyBasket.API.Services
{
    public class PollService : IPollService
    { 
        private static readonly List<Poll> _poll = new List<Poll>();
      
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ICacheService _cacheService;
        private readonly ILogger<PollService> _logger;
        private readonly INotificationService _notificationService;

        public PollService(ApplicationDbContext applicationDbContext,ICacheService cacheService,ILogger<PollService> logger, INotificationService notificationService)
        {
            _applicationDbContext = applicationDbContext;
            _cacheService = cacheService;
            _logger = logger;
            _notificationService = notificationService;
        }
        private const string _CachePrefix = "avalilablePolls";

        public async Task<Result<IEnumerable<PollResponse>>> GetCurrentPollAsync(CancellationToken cancellationToken= default)
        {
            var GetCurrent = await _applicationDbContext
                .Polls
                .AsNoTracking()
                .Where(x => x.IsPublisher  && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
                .ToListAsync(cancellationToken);

            return Result.Success(GetCurrent.Adapt<IEnumerable<PollResponse>>());
        }
        public async Task<Result<PollResponse>> AddPollAsync([FromBody] PollDtoRequest pollDto, CancellationToken cancellationToken = default)
        {
            if (await _applicationDbContext.Polls.AnyAsync(x => x.Title == pollDto.Title && x.Description == pollDto.Description))
                return Result.Failure<PollResponse>(PollErrors.DublicatedPoll);
            if (pollDto.EndsAt <= pollDto.StartsAt)
                return Result.Failure<PollResponse>(PollErrors.CheckonDate);

           

            var Mapster = pollDto.Adapt<Poll>();
            await _applicationDbContext.Polls.AddAsync(Mapster); 
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            await _cacheService.RemoveAsync(_CachePrefix, cancellationToken);

            return Result.Success(Mapster.Adapt<PollResponse>());

        }

        public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var cacheKey = $"{_CachePrefix}";
            var cachePolls = await _cacheService.GetAsync<IEnumerable<PollResponse>>(cacheKey, cancellationToken);
            IEnumerable<PollResponse> Polls = [];

            if(cachePolls is null)
            {
              _logger.LogInformation("Select From Database");
               var dbPolls = await _applicationDbContext.Polls
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);

                Polls = dbPolls.Adapt<IEnumerable<PollResponse>>();

                await _cacheService.SetAsync(cacheKey, Polls, cancellationToken);
            }
            else
            {
                _logger.LogInformation("Select From Cache");
                Polls = cachePolls;
            }

            return Result.Success(Polls);
        }

        public async Task<Result<PollResponse>> GetPollByIdAsync(int? Id, CancellationToken cancellationToken = default)
        {

            var pollDto = await _applicationDbContext.Polls.FirstOrDefaultAsync(x => x.Id == Id, cancellationToken);
            if (pollDto is null)
                return Result.Failure<PollResponse>(PollErrors.PollNotFound);
            return Result.Success(pollDto.Adapt<PollResponse>());
        }

        public async Task<Result> UpdatePollAsync(int Id, PollDtoRequest pollDto, CancellationToken cancellationToken = default)
        {
            var find = await _applicationDbContext.Polls.FindAsync(Id, cancellationToken);
            if (find is null)
                return Result.Failure(PollErrors.PollNotFound); 

            if (string.IsNullOrWhiteSpace(pollDto.Title) || string.IsNullOrWhiteSpace(pollDto.Description))
                return Result.Failure(PollErrors.NotEmpty);


                                                       
            var IsDublicate = await _applicationDbContext.Polls.FirstOrDefaultAsync(x => x.Title == pollDto.Title && x.Id != Id);
            if (IsDublicate is not null)
                return Result.Failure(PollErrors.DublicatedPoll);


            find.Title = pollDto.Title;
            find.Description = pollDto.Description;
            find.EndsAt = pollDto.EndsAt;
            find.StartsAt = pollDto.StartsAt;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync(_CachePrefix, cancellationToken);

            return Result.Success();
        }

        public async Task<Result> DeletePollAsync(int Id, CancellationToken cancellationToken = default)
        {

            var find = await _applicationDbContext.Polls.FindAsync(Id, cancellationToken);
            if (find is null)
                return Result.Failure(PollErrors.PollNotFound);

            _applicationDbContext.Remove(find);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> TogglePublishStatusAsync(int Id, CancellationToken cancellationToken = default)
        {
            var find = await _applicationDbContext.Polls.FindAsync(Id);
            if (find is null)
                return Result.Failure(PollErrors.PollNotFound);

            find.IsPublisher = !find.IsPublisher;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            if (find.IsPublisher && find.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
                BackgroundJob.Enqueue(() => _notificationService.SendNewPollsNotification(find.Id));

            return Result.Success();
        }



    }
}
