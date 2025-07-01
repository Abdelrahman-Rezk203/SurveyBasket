using SurveyBasket.API.Abstractions;

namespace SurveyBasket.API.Errors
{
    public class VoteError
    {
        public static readonly Error DublicatedVote =
            new("Vote.DublicatedVote", "This User already Voted For this Poll", StatusCodes.Status409Conflict);

        public static readonly Error InvalidQuestion =
            new("Vote.InvalidQuestion", "Invalid Question", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidAnswers =
          new("Vote.InvalidAnswers", "Invalid AnswerId", StatusCodes.Status400BadRequest);
    }
}
