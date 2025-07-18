﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using OneOf;
using SurveyBasket.API.Authentication;
using SurveyBasket.API.Dto;
using SurveyBasket.API.DtoRequestAndResponse;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Helpers;
using SurveyBasket.API.Repositories;
using System.Security.Cryptography;
using System.Text;
using Hangfire;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Persistance.DbContext;
using SurveyBasket.API.Abstractions.ResultPattern;

namespace SurveyBasket.API.Services
{
    public class AuthService : IAuth
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AuthService> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _dbContext;
        private readonly int _refreshTokenExpirstionDay = 14; //Day
        public AuthService(UserManager<ApplicationUser> userManager,
                           IJwtProvider jwtProvider,
                          IConfiguration configuration,
                          SignInManager<ApplicationUser> signInManager,
                          ILogger<AuthService> logger,
                          IEmailSender emailSender,
                          IHttpContextAccessor httpContextAccessor,
                          ApplicationDbContext dbContext
                          )
        { 
            _userManager = userManager;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }
        public async Task<OneOf<AuthResponse,Error>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
          
            //Check If Found?
            var find = await _userManager.FindByEmailAsync(email);
            if (find is null)
                return UserErrors.InvalidCredential;

            if(find.IsDisabled)
                return UserErrors.DisabledUser;

            ////check Password 
            //var IsValidPass = await _userManager.CheckPasswordAsync(find,password);
            //if (IsValidPass is false)
            //    return UserErrors.InvalidCredential;

            var result = await _signInManager.PasswordSignInAsync(find, password, true, true);

            if (result.IsLockedOut)
                return UserErrors.UserLockedOut;

            if (result.Succeeded)
            {
                var (roles, permissions) = await GetRoleAndPermissionForUser(find,cancellationToken);
                var (token, expiresIn) = _jwtProvider.GenerateToken(find,_configuration,roles,permissions);


            //Refresh Token
            var refreshToken = GenareteRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirstionDay);

            find.RefreshTokens.Add(new RefreshToken 
            {//refreshToken
                Token = refreshToken , 
                ExpireIn = refreshTokenExpiration
            });

             await _userManager.UpdateAsync(find);

            return  new AuthResponse(find.Id, find.FirstName, find.LastName, find.Email!, token, expiresIn, refreshToken , refreshTokenExpiration);

            }

            return result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredential;

        }

        public async Task<OneOf<AuthResponse,Error>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token); // decode the token and check if it matches the one I created, if it matches, it will return the userId, otherwise null

            if (userId is null) //invalid token
                return UserErrors.InvalidToken;

            var user = await _userManager.FindByIdAsync(userId); //get the user by Id   

            if (user is null)      
                return UserErrors.UserNotFound;


            if (user.IsDisabled)
                return UserErrors.DisabledUser;
                                                                     //refreshToken
            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActivate);
                                                                    //sure the refresh token is still active and matches the one I have


            if (userRefreshToken is null) //not found or revoked
                return UserErrors.RefreshTokenNotMatched;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var (roles, permissions) = await GetRoleAndPermissionForUser(user ,cancellationToken);
            //New Token
            var (NewToken, expiresIn) = _jwtProvider.GenerateToken(user, _configuration, roles, permissions);

            //New Refresh Token
            var NewRefreshToken = GenareteRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirstionDay);

            user.RefreshTokens.Add(new RefreshToken
            {//refreshToken
                Token = refreshToken,
                ExpireIn = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);

            return new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email!, NewToken, expiresIn, NewRefreshToken, refreshTokenExpiration);


        }

        public async Task<OneOf<bool,Error>> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token); //decoding the token to get the userId, if it matches the one I created, it will return the userId, otherwise null

            if (userId is null) 
                return UserErrors.RefreshTokenNotMatched;

            var user = await _userManager.FindByIdAsync(userId); 

            if (user is null)    
                return UserErrors.UserNotFound;

            //refreshToken
            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActivate);


            if (userRefreshToken is null) 
                return UserErrors.RefreshTokenRevoked;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return true;
        }

        public async Task<OneOf<bool, Error>> RegisterAsync(RegisterRequest request ,  CancellationToken cancellationToken = default)
        {
            var EmaitIsFound = await _userManager.Users.AnyAsync(x => x.Email == request.Email , cancellationToken);
            if (EmaitIsFound)
                return UserErrors.DublicatedEmail;

            var User = new ApplicationUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                // PasswordHash = request.Password, //incorrect way to set password, use UserManager.CreateAsync instead
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await _userManager.CreateAsync(User, request.Password);

            if(result.Succeeded)
            {
               var code = await _userManager.GenerateEmailConfirmationTokenAsync(User);
               code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


                _logger.LogInformation("Confirmed Code {code}:", code);

                await SendComfirmationEmail(User, code);

               return true;
            }

            var error = result.Errors.First();

            return (new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }

        public async Task<OneOf<bool, Error>> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default)
        {
            if (await _userManager.FindByIdAsync(request.UserId) is not { } User)
                return UserErrors.UserNotFound;

            if (User.EmailConfirmed)
                return UserErrors.DublicatedConfirmation;

            var code = request.Code; 

            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return UserErrors.InvalidCode;
            }

            var result = await _userManager.ConfirmEmailAsync(User, code);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(User, DefaultRoles.Member);
                return true;
            }

            var error = result.Errors.First();
            return new Error(error.Code, error.Description, StatusCodes.Status400BadRequest);
        }

        public async Task<OneOf<bool, Error>> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request, CancellationToken cancellationToken = default)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } User)
                return true;
             
            if(User.EmailConfirmed)
                return UserErrors.DublicatedConfirmation;

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(User);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Confirmed Code {code} : ", code);

            await SendComfirmationEmail(User, code);

            return true;

        }
     
        public async Task<OneOf<bool, Error>> SendResetPasswordCodeAsync(ForgetPasswordRequest forgetPasswordRequest)
        {
            var User = await _userManager.FindByEmailAsync(forgetPasswordRequest.Email);
            if (User == null)
                return true; //to avoid information leakage, we return true even if the user is not found

            var code = await _userManager.GeneratePasswordResetTokenAsync(User);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Reset Code {code} : ", code);

            await SendResetPasswordEmail(User, code);

            return true;

        }

        public async Task<OneOf<bool, Error>> ResetPasswordAsync(ResetPasswordRequest resetPassword)
        {
            var User = await _userManager.FindByEmailAsync(resetPassword.Email);

            if (User == null)
                return UserErrors.InvalidCode;

            else if (!User.EmailConfirmed)
                return UserErrors.EmailNotConfirmed;

                IdentityResult result;
            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPassword.Code));
                result = await _userManager.ResetPasswordAsync(User, code, resetPassword.NewPassword);  
            }
            catch (FormatException)
            {

                result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken()); 
            }

            if (result.Succeeded)
                return true;

           return UserErrors.InvalidToken;

        }

        private static string GenareteRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private async Task SendComfirmationEmail(ApplicationUser User , string Code)
        {
            var origin = _httpContextAccessor?.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailComfirmation", new Dictionary<string, string>
            {
                { "{{name}}",User.FirstName },
                { "{{action_url}}"  , $"{origin}/Auth/ConfirmEmail?userId={User.Id}&Code={Code}"}  
            });

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(User.Email!, "✅ Survey Basket: Email Confirmation ", emailBody));

            await Task.CompletedTask;
        }

        private async Task SendResetPasswordEmail(ApplicationUser User, string Code)
        {
            var origin = _httpContextAccessor?.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword", new Dictionary<string, string>
            {
                { "{{name}}",User.FirstName },
                { "{{action_url}}"  , $"{origin}/Auth/ForgetPassword?email={User.Email}&Code={Code}"}
            });

            BackgroundJob.Enqueue( () => _emailSender.SendEmailAsync(User.Email!, "✅ Survey Basket: Change Password ", emailBody));

            await Task.CompletedTask;
        }


        private async Task<(IEnumerable<string> roles ,IEnumerable<string> permission)> GetRoleAndPermissionForUser(ApplicationUser User , CancellationToken cancellationToken)
        {
            var UserRoles = await _userManager.GetRolesAsync(User);
            var UserPermessions = await _dbContext.Roles
                .Join(_dbContext.RoleClaims,
                roles => roles.Id,
                claims => claims.RoleId,
                (roles, claims) => new { roles, claims }
                )
                .Where(x => UserRoles.Contains(x.roles.Name!))
                .Select(x => x.claims.ClaimValue)
                .Distinct()
                .ToListAsync(cancellationToken);

            return (UserRoles, UserPermessions!);
        }
    }
}
