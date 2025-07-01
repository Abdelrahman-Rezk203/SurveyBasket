using FluentValidation;
using SurveyBasket.API.DtoRequestAndResponse;

namespace SurveyBasket.API.FluentValidations
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x=>x.Code)
                .NotEmpty();
        }
    }
}
