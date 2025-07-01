using FluentValidation;
using SurveyBasket.API.Dto.Polls;
using SurveyBasket.API.DtoRequestAndResponse.Question;
using SurveyBasket.API.Entities;
using System;

namespace SurveyBasket.API.FluentValidations
{
    public class VoteAnswerValidation : AbstractValidator<VoteAnswer>
    {
        public VoteAnswerValidation()
        {
            //RuleFor(x => new {x.Id,x.p})


             
            // */
        }



    }
}
