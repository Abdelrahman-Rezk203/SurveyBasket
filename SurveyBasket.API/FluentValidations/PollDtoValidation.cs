using FluentValidation;
using SurveyBasket.API.Dto.Polls;
using System;

namespace SurveyBasket.API.FluentValidations
{
    public class PollDtoValidation : AbstractValidator<PollDtoRequest>
    {
        public PollDtoValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty()//بتطبع رساله من نفسها 
                .Length(3, 10)
                .WithMessage("Title Should be at least {MinLength} , you entered [{PropertyValue}]");

            RuleFor(x => x.Description)
                .NotEmpty()
                .Length(3, 1000);

            RuleFor(x => x.EndsAt)
                .NotEmpty();


            RuleFor(x => x.StartsAt)
                 .NotEmpty()           //عشان مينفعش ابدا في وقت ماضي لازم البدايه اكبر من تاريخ اليوم 
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)); 

            RuleFor(x => x)
                .Must(EndAtGreaterThanStartAt)
                .WithName(nameof(PollDtoRequest.EndsAt))
                .WithMessage("{PropertyName} Must be Greater Than or Equal StartAt");
              /* WithName و WithMessage شارح 
             {
                  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                  "title": "One or more validation errors occurred.",
                  "status": 400,
                  "errors": {
                    "EndsAt": [ // WithName(nameof(PollDto.EndsAt)) عشان الاسم ده يظهر عملت
                      "EndsAt Must be Greater Than or Equal StartAt"  //WithMessage دا عشان الرساله تظهر 
                    ]
                  },
                  "traceId": "00-4d2160691ed7f59250eef653f14bf0bc-dc705795d4477f36-00"
            }
             */




        }

        private bool EndAtGreaterThanStartAt(PollDtoRequest pollDto)
        {
            return pollDto.EndsAt >= pollDto.StartsAt;
        }

       
    }
}
