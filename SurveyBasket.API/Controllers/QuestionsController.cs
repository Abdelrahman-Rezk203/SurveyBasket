using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.DtoRequestAndResponse.Question;
using SurveyBasket.API.Repositories;
using SurveyBasket.Authentication.Filters;

namespace SurveyBasket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost("AddQuestion/{PollId}")]
        [HasPermission(Permissions.AddQuestion)]
        public async Task<IActionResult> AddQuestion([FromRoute] int PollId, QuestionRequest questionRequest, CancellationToken cancellationToken = default)
        {
            var AddedQuestion = await _questionService.AddQuestionAsync(PollId, questionRequest, cancellationToken);
            return AddedQuestion.IsSuccess ? Ok(AddedQuestion.Value) : AddedQuestion.ToProblem();
        }

        [HttpGet("GetAllQuestions/{PollId}")]
        [HasPermission(Permissions.GetQuestion)]
        public async Task<IActionResult> GatAllQuestion([FromRoute] int PollId , CancellationToken cancellationToken = default)
        {
            var UpdatedQuestion = await _questionService.GetAllQuestionsAsync(PollId ,cancellationToken);
            return UpdatedQuestion.IsSuccess ? Ok(UpdatedQuestion.Value) : UpdatedQuestion.ToProblem();
        }

        [HttpGet("GetQuestionById/{PollId}/{Id}")]
        [HasPermission(Permissions.GetQuestion)]
        public async Task<IActionResult> GetQuestionById([FromRoute] int PollId , [FromRoute] int Id ,CancellationToken cancellationToken = default)
        {
            var GetById = await _questionService.GetQuestionsByIdAsync(PollId, Id,cancellationToken);
            return GetById.IsSuccess ? Ok(GetById.Value) : GetById.ToProblem();
        }

        [HttpPut("AddToggleQuestionStatus/{PollId}/{Id}")]
        [HasPermission(Permissions.UpdateQuestion)]
        public async Task<IActionResult> AddToggleQuestionStatusAsync([FromRoute] int PollId, [FromRoute] int Id, CancellationToken cancellationToken = default)
        {
            var UpdateStatusForQuestion = await _questionService.AddToggleQuestionsStatusAsync(PollId, Id, cancellationToken);
            return UpdateStatusForQuestion.IsSuccess ? Ok(UpdateStatusForQuestion.Value) : UpdateStatusForQuestion.ToProblem();
        }

        [HttpPut("AddToggleAnswersStatus/{pollId}/{QuestionId}/{AnswerId}")]
        [HasPermission(Permissions.UpdateToggleAnswers)] 
        public async Task<IActionResult> AddToggleAnswersStatusAsync([FromRoute] int pollId , [FromRoute] int QuestionId , [FromRoute] int AnswerId , CancellationToken cancellationToken = default)
        {
            var UpdateStatusForAnswer = await _questionService.AddToggleAnswersStatusAsync(pollId, QuestionId, AnswerId, cancellationToken);
            return UpdateStatusForAnswer.IsSuccess ? NoContent() : UpdateStatusForAnswer.ToProblem();
        }


        [HttpPut("UpdateQuestion/{PollId}/{Id}")]
        [HasPermission(Permissions.UpdateQuestion)]
        public async Task<IActionResult> UpdateQuestion([FromRoute] int PollId , [FromRoute] int Id , QuestionRequest questionRequest , CancellationToken cancellationToken)
        {
            var UpdateQuestion = await _questionService.UpdateQuestion(PollId, Id,questionRequest , cancellationToken);
            return UpdateQuestion.IsSuccess ? NoContent() : UpdateQuestion.ToProblem();
        }

      
    }
}
