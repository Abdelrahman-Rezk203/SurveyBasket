
//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Authentication;
using SurveyBasket.API.Dto;
using SurveyBasket.API.DtoRequestAndResponse;
using SurveyBasket.API.Repositories;
//using RegisterRequest = SurveyBasket.API.DtoRequestAndResponse.RegisterRequest;
//using ResendConfirmationEmailRequest = SurveyBasket.API.DtoRequestAndResponse.ResendConfirmationEmailRequest;

namespace SurveyBasket.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _auth;
        private readonly ILogger<AuthController> _logger;
        private readonly JwtOptionPattern _options; //class name only not interface 

        //private readonly IConfiguration _configuration;

        public AuthController(IAuth auth, IOptions<JwtOptionPattern> options        /*,IConfiguration configuration*/ , ILogger<AuthController> logger)
        {
            _auth = auth;
            _logger = logger;
            _options = options.Value;
            //_configuration = configuration;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest authRequest, CancellationToken cancellationToken)
        {
            //_logger.LogInformation($" Logging with email: {authRequest.Email} and Password: {authRequest.Password}");//string intepolation  صعبه ف السيرش

            _logger.LogInformation("Logging with email: {email} and password: {password} ", authRequest.Email, authRequest.Password);

            var authResult = await _auth.GetTokenAsync(authRequest.Email, authRequest.Password, cancellationToken);
            return authResult.Match(
                   AuthResult => Ok(authResult.Value),
                   error => Problem(statusCode: StatusCodes.Status400BadRequest, title: error.Code, detail: error.Description)
                );
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken = default)
        {
            var refreshResult = await _auth.GetRefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken, cancellationToken);
            return refreshResult.Match(
                  Ok,
                 errorFuntion
            );
        }
        IActionResult errorFuntion(Error error)  //Function عاديه 
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: error.Code, detail: error.Description);
        }

        IActionResult errorFuntionRegister(Error error)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: error.Code, detail: error.Description);
        }


        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest RevokerefreshTokenRequest, CancellationToken cancellationToken = default)
        {
            var IsRevoked = await _auth.RevokeRefreshTokenAsync(RevokerefreshTokenRequest.Token, RevokerefreshTokenRequest.RefreshToken, cancellationToken);
            return IsRevoked.Match(
                isRevoked => Ok(isRevoked),
                error => Problem(statusCode: StatusCodes.Status400BadRequest, title: error.Code, detail: error.Description)
                );
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var User = await _auth.RegisterAsync(request, cancellationToken);
            return User.Match(
                  x => Ok(),  //OneOf عشان بتستخدم ال 
                  errorFuntionRegister
               );

        }

        [HttpPost("Confirmation-Email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken = default)
        {
            var User = await _auth.ConfirmEmailAsync(request, cancellationToken);
            return User.Match<IActionResult>(
                  x => Ok(),  //OneOf عشان بتستخدم ال 
                  error => Problem(statusCode: StatusCodes.Status400BadRequest, title: error.Code, detail: error.Description)
               );

        }


        [HttpPost("Resend-Confirmation-Email")]
        public async Task<IActionResult> ResendConfirmEmail([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellationToken = default)
        {
            var ResendConfimmation = await _auth.ResendConfirmationEmailAsync(request, cancellationToken);
            return ResendConfimmation.Match<IActionResult>(
                x => Ok(),
                error => Problem(statusCode: StatusCodes.Status400BadRequest, title: error.Code, detail: error.Description)
                );

        }

        [HttpPost("Forget-Password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            var ForgetPassword = await _auth.SendResetPasswordCodeAsync(request);
            return ForgetPassword.Match<IActionResult>(
                x => Ok(),
                error => Problem(statusCode: StatusCodes.Status400BadRequest, title: error.Code, detail: error.Description)
                );

        }


        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest  resetPassword)
        {
            var ResetPassword = await _auth.ResetPasswordAsync(resetPassword);
            return ResetPassword.Match<IActionResult>(
                x => Ok(),
                error => Problem(statusCode: StatusCodes.Status400BadRequest, title: error.Code, detail: error.Description)
                );

        }









            /// [HttpGet("")]
            /// public  IActionResult test()
            /// {
            ///     var configuration = new
            ///     {
            ///         MyKey = _configuration["MyKey"],
            ///         Enviornment = _configuration["ASPNETCORE_ENVIRONMENT"],
            ///         OneDrive = _configuration["OneDrive"] //On My PC
            ///     };
            ///
            ///     return Ok(configuration);
            /// }
    }
}