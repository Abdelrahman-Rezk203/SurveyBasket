using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Abstractions.ResultPattern;
using SurveyBasket.API.Dto.Polls;
using SurveyBasket.API.Repositories;
using SurveyBasket.Authentication.Filters;

namespace SurveyBasket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PollsController : ControllerBase
    {
        private readonly IPollService _pollServices;

        public PollsController(IPollService pollServices)
        {
            _pollServices = pollServices;
        }

        [HttpGet("GetAllCurrentPoll")]
        [Authorize(Roles = DefaultRoles.Member)]
        public async Task<IActionResult> GetAllCurrentPoll(CancellationToken cancellationToken =default )
        {
            var GetCurrent = await _pollServices.GetCurrentPollAsync(cancellationToken);
            return GetCurrent.IsSuccess ? Ok(GetCurrent.Value) : GetCurrent.ToProblem();
        }

        [HttpGet("GetAll")]
        [HasPermission(Permissions.GetPoll)]
        public async Task<IActionResult> GetAllPolls(CancellationToken cancellationToken = default)
        {
            var poll = await _pollServices.GetAllAsync(cancellationToken);
            return poll.IsSuccess ? Ok(poll.Value) : poll.ToProblem() ;      
        }


        [HttpGet("GetId/{Id}")]
        [HasPermission(Permissions.GetPoll)]
        public async Task<IActionResult> GetPollById([FromRoute] int Id, CancellationToken cancellationToken = default)
        {
           
            var find = await _pollServices.GetPollByIdAsync(Id, cancellationToken);
            return find.IsSuccess ? Ok(find.Value) : find.ToProblem();             
        }


        [HttpPost("AddNewPoll")]
        [HasPermission(Permissions.AddPoll)]
        public async Task<IActionResult> AddPoll([FromBody] PollDtoRequest pollDto, CancellationToken cancellationToken = default)
        {
            var AddedPoll = await _pollServices.AddPollAsync(pollDto, cancellationToken);
            
            return AddedPoll.IsSuccess ? Ok(AddedPoll.Value) : AddedPoll.ToProblem();
        }

        [HttpPut("UpdatePoll/{Id}")]
        [HasPermission(Permissions.UpdatePoll)]
        public async Task<IActionResult> UpdatePoll([FromRoute] int Id, [FromBody] PollDtoRequest pollDto, CancellationToken cancellationToken = default)
        {
            var UpdatedPoll = await _pollServices.UpdatePollAsync(Id, pollDto, cancellationToken);

            return UpdatedPoll.IsSuccess ? NoContent() : UpdatedPoll.ToProblem();

        }

        [HttpDelete("DeletePoll/{Id}")]
        [HasPermission(Permissions.DeletePoll)]
        public async Task<IActionResult> DeletePoll([FromRoute] int Id, CancellationToken cancellationToken = default)
        {
            var PollDeleted = await _pollServices.DeletePollAsync(Id, cancellationToken);
            return PollDeleted.IsSuccess ? NoContent() : NotFound(PollDeleted.Error);

        }


        [HttpPut("{Id}/TogglePublish")] 
        [HasPermission(Permissions.UpdatePoll)]
        public async Task<IActionResult> TogglePublishStatus(int Id, CancellationToken cancellationToken = default)
        { 
            var Published = await _pollServices.TogglePublishStatusAsync(Id, cancellationToken);
            return Published.IsSuccess ? NoContent() : Published.ToProblem();

        }
































        /*
        #region تطبيق علي المابستر
        [HttpGet("Convert_SameValues")]
        public async Task<IActionResult> test()
        {
            var student = new StudentDtoRequest
            {
                Id = 1,
                FirstName = "Abdelrahman",
                MiddleName = "Rezk",
                LastName = "Mohamed",
                DateOFBirth = new DateTime(2003,5,10),
                Department = new DepartmentDtoRequest
                {
                    Name = "Sales"
                }
            };

            //MappingConfiguration طالما نفس الداات بين الاتنين كلاس اللي هحول بينهم يبقي مش لازم اروح لل 
            var mapping = student.Adapt<StudentResponse>();
            return Ok(mapping);
        }

        #endregion

        #region Test For Validation
        [HttpPost("test")]
        public async Task<IActionResult> test([FromBody] StudentDtoRequest studentDtoRequest)
        {
            return Ok("Accepted");
        } 
        #endregion

        */
    }
}
