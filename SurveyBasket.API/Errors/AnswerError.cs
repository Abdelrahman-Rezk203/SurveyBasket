using SurveyBasket.API.Abstractions;

namespace SurveyBasket.API.Errors
{
    public static class AnswerError
    {
        public static readonly Error AnswerNotFound =
            new("Answer.NotFound" , "This Answer Can't Exist in This Question" , StatusCodes.Status404NotFound);
    }
}
