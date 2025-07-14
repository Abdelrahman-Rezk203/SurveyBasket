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
    [Authorize]
    //هيدي اكسبشن CreatedById is required وانت عامل ال  null هيكون ب    UserId لو معملتهاش مش هيستخدم التوكن وبالتالي مش هيقدر يجيب ال 
    public class PollsController : ControllerBase
    {
        private readonly IPollService _pollServices;

        public PollsController(IPollService pollServices)
        {
            _pollServices = pollServices;
        }


        //لازم الكنستراكتور يكون بابلك عشان الكومبايلر يشوفه ويقدر يكريت منه اوبجيكت لما يحتاجه 
        /// public PollsController(List<Poll> poll) //هنا انت عملت انجيكت يبقي لازم اروح اعملها في البروجيرام
        /// {
        ///     _poll = poll; //عشان هو لما يعمل ابوجيكت من الكلاس هتبقي فاضيه  null الكنستراكتور بيكون فاضي 
        ///
        /// }
        [Authorize(Roles = DefaultRoles.Member)]
        [HttpGet("GetAllCurrentPoll")]
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
        public async Task<IActionResult> GetPollById([FromRoute] int Id, CancellationToken cancellationToken = default)
        {
            //var find = _poll.SingleOrDefault(x => x.Id == Id);
            ////if (find is null) return NotFound();

            ////return Ok(find);
            var find = await _pollServices.GetPollByIdAsync(Id, cancellationToken);
            return find.IsSuccess ? Ok(find.Value) : find.ToProblem();             
        }


        [HttpPost("")]
        public async Task<IActionResult> AddPoll([FromBody] PollDtoRequest pollDto, CancellationToken cancellationToken = default)
        {
            //علي الكلاس  FluentValidation كده كده انا عامل فاليديشن بال 
            var AddedPoll = await _pollServices.AddPollAsync(pollDto, cancellationToken);
            //return Ok("Added Successfully");
            //return CreatedAtAction(nameof(GetPollById),new { id = x.Id } ,x.Adapt<PollResponse>());
            /*
                                               Response body   
                                                {
                                                  "id": 4,
                                                  "title": "string",
                                                  "description": "string"
                                                }

                                               Response headers
                                                 content-type: application/json; charset=utf-8 
                                                 date: Tue,18 Feb 2025 04:34:49 GMT 
                                                 location: https://localhost:7287/api/Polls/GetId/4 بترجع مكان الاضافه
                                                 server: Kestrel 
             */

            return AddedPoll.IsSuccess ? Ok(AddedPoll.Value) : AddedPoll.ToProblem();
        }

        [HttpPut("UpdatePoll/{Id}")]
        public async Task<IActionResult> UpdatePoll([FromRoute] int Id, [FromBody] PollDtoRequest pollDto, CancellationToken cancellationToken = default)
        {
            //null انا مش هحط شرط هنا لان كدا كدا انا هعدل علي حاجه موجوده اساسا مينفعش تكون 
            var UpdatedPoll = await _pollServices.UpdatePollAsync(Id, pollDto, cancellationToken);

            return UpdatedPoll.IsSuccess ? NoContent() : UpdatedPoll.ToProblem();

        }

        [HttpDelete("DeletePoll/{Id}")]
        public async Task<IActionResult> DeletePoll([FromRoute] int Id, CancellationToken cancellationToken = default)
        {
            var PollDeleted = await _pollServices.DeletePollAsync(Id, cancellationToken);
            return PollDeleted.IsSuccess ? NoContent() : NotFound(PollDeleted.Error);

        }


        [HttpPut("{Id}/TogglePublish")]
        public async Task<IActionResult> TogglePublishStatus(int Id, CancellationToken cancellationToken = default)
        { //عملت كده عشان الفانكشن بترجع ترو او فالس ف انا بقوله لو رجعت ترو اعمل كذا ولو فالس اعمل كذا 
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
