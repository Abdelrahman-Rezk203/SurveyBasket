using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.API.Dto.Polls;
using SurveyBasket.API.DtoRequestAndResponse.Question;
using SurveyBasket.API.DtoRequestAndResponse.Vote;
using SurveyBasket.API.Entities;
using System;

namespace SurveyBasket.API.FluentValidations
{
    public class VoteRequestValidation : AbstractValidator<VoteRequest>
    {
        public VoteRequestValidation()
        {
            RuleFor(x => x.Answers)
                .NotEmpty();

            RuleForEach(x => x.Answers).SetInheritanceValidator(v => v.Add(new VoteAnswerRequestValidation()));
        }
    }
}
