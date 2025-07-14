using SurveyBasket.API.Abstractions.ResultPattern;
using System.Security.Authentication;

namespace SurveyBasket.API.Errors
{
    public static class UserErrors
    {
      
        public static readonly Error InvalidCredential = 
            new ("User.InvalidCredintial", "Invalid email/password",StatusCodes.Status401Unauthorized);
        
        public static readonly Error DublicatedEmail = 
            new ("User.DublicatedEmail", "Another User Registered With The Same Email , Email Is Alredy Exist",StatusCodes.Status404NotFound);
       
        public static readonly Error EmailNotConfirmed = 
            new ("User.EmailNotConfirmed", "Email Is Not Confirmed", StatusCodes.Status401Unauthorized);
     
        public static readonly Error DublicatedConfirmation = 
            new ("User.DublicatedConfirmation", "This Email already Confirmed", StatusCodes.Status401Unauthorized);
        
        public static readonly Error InvalidCode = 
            new ("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);
        
        public static readonly Error InvalidToken = 
            new("User.InvalidToken", "This Token Is Invalid , Enter Valid Data", StatusCodes.Status401Unauthorized);
        
        public static readonly Error UserNotFound = 
            new("User.UserNotFound", " User Not Found", StatusCodes.Status404NotFound);

        public static readonly Error RefreshTokenRevoked = 
            new("User.RefreshTokenNotFound", "Refresh token not found or already revoked.", StatusCodes.Status401Unauthorized);
        
        public static readonly Error RefreshTokenNotMatched = 
            new("User.RefreshTokenRevoked", "Refresh token is missing, invalid, or does not match our records.", StatusCodes.Status401Unauthorized);
       
        public static readonly Error ChangePaswordInvalid = 
            new("User.ChangePaswordInvalid ", "Invalid CurrentPassword", StatusCodes.Status400BadRequest);
        
        public static readonly Error UserLockedOut = 
            new("User.UserLockedOut ", "Locked User, Please Contact Your Administrator", StatusCodes.Status400BadRequest);
       
        public static readonly Error DisabledUser = 
            new("User.DisabledUser ", "Disabled User, Please Contact Your Administrator", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidRoles =
           new(Code: "User. InvalidRoles", Description: "Invalid Roles", StatusCodes.Status400BadRequest);


        public static readonly Error FailureAddedUser =
           new(Code: "User. FailureAddedUser", Description: "Failure Added User", StatusCodes.Status400BadRequest);






    }
}
