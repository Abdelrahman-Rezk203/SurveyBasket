using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Abstractions.ResultPattern;
using SurveyBasket.API.Repositories;
using SurveyBasket.Authentication.Filters;

namespace SurveyBasket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [HasPermission(Permissions.Result)]
    public class ResultVotesController : ControllerBase
    {
        private readonly IResultVote _resultVote;

        public ResultVotesController(IResultVote resultVote)
        {
            _resultVote = resultVote;
        }

        [HttpGet("GetVoteResult/{pollId}")]
        public async Task<ActionResult> GetVoteResult([FromRoute] int pollId, CancellationToken cancellationToken = default)
        {
            var GetVoteResult = await _resultVote.GetVoteResultAsync(pollId, cancellationToken);
            return GetVoteResult.IsSuccess ? Ok(GetVoteResult.Value) : GetVoteResult.ToProblem();
        }

        [HttpGet("GetVotePerDay/{pollId}")]
        public async Task<ActionResult> GetVotePerDay([FromRoute] int pollId, CancellationToken cancellationToken = default)
        {
            var GetVotePerDay = await _resultVote.GetVotePerDayAsync(pollId, cancellationToken);
            return GetVotePerDay.IsSuccess ? Ok(GetVotePerDay.Value) : GetVotePerDay.ToProblem();
        }

        [HttpGet("GetVotePerQuestion/{pollId}")]
        public async Task<ActionResult> GetVotePerQuestion([FromRoute] int pollId, CancellationToken cancellationToken = default)
        {
            var GetVotePerQuestion = await _resultVote.GetVotePerQuestionAsync(pollId, cancellationToken);
            return GetVotePerQuestion.IsSuccess ? Ok(GetVotePerQuestion.Value) : GetVotePerQuestion.ToProblem();
        }

    }
}
