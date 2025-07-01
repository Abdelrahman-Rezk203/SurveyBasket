using SurveyBasket.API.Abstractions;

namespace SurveyBasket.API.Errors
{
    public class QuestionError
    {
        public static readonly Error ContentNotEmpty = 
            new ("Question.NotEmpty","Content Can't Be Empty",StatusCodes.Status400BadRequest);

         public static readonly Error QuestionNotFound = 
            new ("Question.QuestionNotFound", "This Question Not Found",StatusCodes.Status404NotFound);

         public static readonly Error QuestionNotFoundOrNotActive = 
            new ("Question.QuestionNotFoundOrNotActive", "This Question Not Found Or Not Active",StatusCodes.Status404NotFound);

        public static readonly Error ContentDublicated = 
            new ("Question.ContentDublicated", "Content Can't Be Empty", StatusCodes.Status400BadRequest);



    }
}
