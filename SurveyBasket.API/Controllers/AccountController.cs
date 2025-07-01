using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.DtoRequestAndResponse;
using SurveyBasket.API.Extentions;
using SurveyBasket.API.Repositories;

namespace SurveyBasket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserProfileService _userProfile;

        public AccountController(IUserProfileService userProfile)
        {
            _userProfile = userProfile;
        }
        [HttpGet("GetUserProfile")]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken = default)
        {
            var GetInfo = await _userProfile.GetUserProfile(UserExtention.GetUserId(User)!, cancellationToken);
            return GetInfo.IsSuccess ? Ok(GetInfo.Value) : GetInfo.ToProblem();
        }

        [HttpPost("UpdteUserProfile")]
        public async Task<IActionResult> UpdateProfile(UserProfileRequest profileRequest , CancellationToken cancellationToken = default)
        {
            var UpdateInfo = await _userProfile.UpdateUserProfile(UserExtention.GetUserId(User)!, profileRequest,cancellationToken);
            return UpdateInfo.IsSuccess ? NoContent() : UpdateInfo.ToProblem();
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest  changePassword , CancellationToken cancellationToken = default)
        {
            var changePass = await _userProfile.ChangePassword(UserExtention.GetUserId(User)!, changePassword, cancellationToken);
            return changePass.IsSuccess ? NoContent() : changePass.ToProblem();
        }



    }
}
