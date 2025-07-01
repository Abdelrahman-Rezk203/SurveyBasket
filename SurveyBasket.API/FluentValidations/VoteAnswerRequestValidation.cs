using FluentValidation;
using SurveyBasket.API.Dto.Polls;
using SurveyBasket.API.DtoRequestAndResponse.VoteAnswer;
using System;

namespace SurveyBasket.API.FluentValidations
{
    public class VoteAnswerRequestValidation : AbstractValidator<VoteAnswerRequest>
    {
        public VoteAnswerRequestValidation()
        {

            RuleFor(x => x.AnswerId)
                .GreaterThan(0);


              RuleFor(x => x.QuestionId)
                .GreaterThan(0);

        }



    }
}
