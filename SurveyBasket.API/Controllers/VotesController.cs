using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.DtoRequestAndResponse.Vote;
using SurveyBasket.API.DtoRequestAndResponse.VoteAnswer;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Extentions;
using SurveyBasket.API.Persistance.DbContext;
using SurveyBasket.API.Repositories;
using SurveyBasket.API.Services;
using System.Security.Claims;

namespace SurveyBasket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
   
    public class VotesController : ControllerBase
    {
        private readonly IVoteSevice _voteSevice;
        private readonly IQuestionService _questionService;

        public VotesController(IVoteSevice voteSevice , IQuestionService questionService )
        {
            _voteSevice = voteSevice;
            _questionService = questionService;
        }

        [HttpGet("GetAvailableQuestionInPoll/{PollId}")]
        public async Task<IActionResult> GetAvailableQuestionToVote([FromRoute] int PollId, CancellationToken cancellationToken = default)
        {                       //Extention Method Created By Me
            var UserId = /*"6dc6528a-b280-4770-9eae-82671ee81ef7"*/ User.GetUserId()!;  //GetUserId From JWT Token 
            var GetAvailabeQuestion = await _questionService.GetAvailableQuestionToVoteAsync(PollId, UserId, cancellationToken);
            return GetAvailabeQuestion.IsSuccess ? Ok(GetAvailabeQuestion.Value) : GetAvailabeQuestion.ToProblem();
        }

        [HttpPost("AddVote/{PollId}")]
        public async Task<IActionResult> AddVoteAsync([FromRoute] int PollId, VoteRequest voteRequest, CancellationToken cancellationToken = default )
        {
            var UserId = User.GetUserId()!;
            var AddVote = await _voteSevice.AddVoteAsync(PollId, UserId , voteRequest , cancellationToken);
            return AddVote.IsSuccess ? NoContent() : AddVote.ToProblem();
        }
    }
}
