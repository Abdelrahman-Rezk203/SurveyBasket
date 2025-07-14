using SurveyBasket.API.Abstractions.ResultPattern;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Errors
{
    public static class PollErrors
    {
        public static readonly Error PollNotFound =
            new("Poll.NotFound" , "No Poll Was Found With The given ID", StatusCodes.Status404NotFound);

        public static readonly Error DublicatedPoll =
            new("Poll.DublicatedPoll", "This Poll already exist", StatusCodes.Status409Conflict);

        public static readonly Error NotEmpty =
            new("Poll.NotEmpty", "Poll Can Not Be Empty",StatusCodes.Status400BadRequest);

         public static readonly Error CheckonDate =
            new("Poll.CheckonDate", "End date must be after start date.", StatusCodes.Status400BadRequest);

         public static readonly Error PollIdNotFound =
            new("Poll.PollIdNotFound", "This Id Not Found In All Polls , Can't Add Question ", StatusCodes.Status400BadRequest);
          
        
        public static readonly Error PollNotFoundOrRevoked =
            new("Poll.PollNotFoundOrRevoked", "This Poll Not Found Or Revoked ", StatusCodes.Status404NotFound);


    }
}
