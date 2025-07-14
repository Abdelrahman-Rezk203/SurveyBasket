using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Abstractions.ResultPattern;
using SurveyBasket.API.DtoRequestAndResponse.Users;
using SurveyBasket.API.Repositories;
using SurveyBasket.Authentication.Filters;

namespace SurveyBasket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            this._userService = userService;
        }
        [HttpGet("GetAllUsers")]
        [HasPermission(Permissions.GetUsers)] 
        public async Task<IActionResult> GetAllUsers([FromQuery] bool? IncludeDisabled = false, CancellationToken cancellationToken = default)
        {
            var GetAllUsers = await _userService.GetAllUsersAsync(IncludeDisabled, cancellationToken);
            return GetAllUsers.IsSuccess ? Ok(GetAllUsers.Value) : GetAllUsers.ToProblem();
        }

        [HttpGet("GetUser/{Id}")]
        [HasPermission(Permissions.GetUsers)]
        public async Task<IActionResult> GetUser([FromRoute] string Id, CancellationToken cancellationToken = default)
        {
            var GetUser = await _userService.GetUserAsync(Id, cancellationToken);
            return GetUser.IsSuccess ? Ok(GetUser.Value) : GetUser.ToProblem();
        }

        [HttpPost("AddNewUser")]
        [HasPermission(Permissions.AddUsers)]
        public async Task<IActionResult> AddNewUser([FromBody] CreateUserRequest createUserRequest, CancellationToken cancellationToken = default)
        {
            var AddUser = await _userService.AddNewUserAsync(createUserRequest, cancellationToken);
            return AddUser.IsSuccess ? Ok(AddUser.Value) : AddUser.ToProblem();
        }


        [HttpPut("UpdateUser/{Id}")]
        [HasPermission(Permissions.UpdateUsers)] 
        public async Task<IActionResult> UpdateUser([FromRoute] string Id ,[FromBody] UpdateUserRequest  updateUserRequest, CancellationToken cancellationToken = default)
        {
            var UpdateUser = await _userService.UpdateUserAsync(Id, updateUserRequest, cancellationToken);
            return UpdateUser.IsSuccess ? NoContent() : UpdateUser.ToProblem();
        }

        [HttpPut("AddToggleStatusUser/{Id}")]
        [HasPermission(Permissions.UpdateUsers)] 
        public async Task<IActionResult> AddToggleStatusUser([FromRoute] string Id , CancellationToken cancellationToken = default)
        {
            var ToggleStatus = await _userService.AddToggleStatusUserAsync(Id, cancellationToken);
            return ToggleStatus.IsSuccess ? NoContent() : ToggleStatus.ToProblem();
        }

        [HttpPut("UnLockUser/{Id}")]
        [HasPermission(Permissions.UpdateUsers)] 
        public async Task<IActionResult> UnLockUser([FromRoute] string Id , CancellationToken cancellationToken = default)
        {
            var Unlock = await _userService.UnLockUserAsync(Id, cancellationToken);
            return Unlock.IsSuccess ? NoContent() : Unlock.ToProblem();
        }




    }
}
