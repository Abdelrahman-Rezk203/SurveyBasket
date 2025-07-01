using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.DtoRequestAndResponse.Question;
using SurveyBasket.API.Repositories;

namespace SurveyBasket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost("AddQuestion/{PollId}")]
        public async Task<IActionResult> AddQuestion([FromRoute] int PollId, QuestionRequest questionRequest, CancellationToken cancellationToken = default)
        {
            var AddedQuestion = await _questionService.AddQuestionAsync(PollId, questionRequest, cancellationToken);
            return AddedQuestion.IsSuccess ? Ok(AddedQuestion.Value) : AddedQuestion.ToProblem();
        }

        [HttpGet("GetAllQuestions/{PollId}")]
        public async Task<IActionResult> GatAllQuestion([FromRoute] int PollId , CancellationToken cancellationToken = default)
        {
            var UpdatedQuestion = await _questionService.GetAllQuestionsAsync(PollId ,cancellationToken);
            return UpdatedQuestion.IsSuccess ? Ok(UpdatedQuestion.Value) : UpdatedQuestion.ToProblem();
        }

        [HttpGet("GetQuestionById/{PollId}/{Id}")]
        public async Task<IActionResult> GetQuestionById([FromRoute] int PollId , [FromRoute] int Id ,CancellationToken cancellationToken = default)
        {
            var GetById = await _questionService.GetQuestionsByIdAsync(PollId, Id,cancellationToken);
            return GetById.IsSuccess ? Ok(GetById.Value) : GetById.ToProblem();
        }

        [HttpPut("AddToggleQuestionStatus/{PollId}/{Id}")]
        public async Task<IActionResult> AddToggleQuestionStatusAsync([FromRoute] int PollId, [FromRoute] int Id, CancellationToken cancellationToken = default)
        {
            var UpdateStatusForQuestion = await _questionService.AddToggleQuestionsStatusAsync(PollId, Id, cancellationToken);
            return UpdateStatusForQuestion.IsSuccess ? Ok(UpdateStatusForQuestion.Value) : UpdateStatusForQuestion.ToProblem();
        }

        [HttpPut("AddToggleAnswersStatus/{pollId}/{QuestionId}/{AnswerId}")]
        public async Task<IActionResult> AddToggleAnswersStatusAsync([FromRoute] int pollId , [FromRoute] int QuestionId , [FromRoute] int AnswerId , CancellationToken cancellationToken = default)
        {
            var UpdateStatusForAnswer = await _questionService.AddToggleAnswersStatusAsync(pollId, QuestionId, AnswerId, cancellationToken);
            return UpdateStatusForAnswer.IsSuccess ? NoContent() : UpdateStatusForAnswer.ToProblem();
        }


        [HttpPut("UpdateQuestion/{PollId}/{Id}")]
        public async Task<IActionResult> UpdateQuestion([FromRoute] int PollId , [FromRoute] int Id , QuestionRequest questionRequest , CancellationToken cancellationToken)
        {
            var UpdateQuestion = await _questionService.UpdateQuestion(PollId, Id,questionRequest , cancellationToken);
            return UpdateQuestion.IsSuccess ? NoContent() : UpdateQuestion.ToProblem();
        }

      
    }
}
