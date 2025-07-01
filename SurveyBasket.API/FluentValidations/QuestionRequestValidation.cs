using FluentValidation;
using SurveyBasket.API.Dto.Polls;
using SurveyBasket.API.DtoRequestAndResponse.Question;
using System;

namespace SurveyBasket.API.FluentValidations
{
    public class QuestionRequestValidation : AbstractValidator<QuestionRequest>
    {
        public QuestionRequestValidation()
        {
            RuleFor(x => x.Content)
                .NotNull()
                .Length(3, 1000)
                .WithMessage("Content Should be at least {MinLength} , you entered [{PropertyValue}]");


            RuleFor(x => x.Answers)
                .NotNull();


            RuleFor(x => x.Answers) //عشان اعنا عامل ليست 
                .Must(x => x.Count > 1)
                .WithMessage("Question Should has at 2 Answers ")
                .When(x=>x.Answers != null);


            RuleFor(x => x.Answers)
                .Must(x => x.Distinct().Count() == x.Count) 
                .WithMessage("You Can't Add Duplicated Answer For The Same Question")
                .When(x=>x.Answers != null);

            /*
             x.Count: عدد العناصر الاسئله اللي جايه ف الريكويست.
             x.Distinct().Count(): عدد العناصر بعد إزالة التكرار.

            لو الرقمين مش متساويين، يبقى فيه قيم مكررة.



             
             */
        }



    }
}
