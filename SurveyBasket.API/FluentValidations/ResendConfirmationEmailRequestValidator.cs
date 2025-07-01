using FluentValidation;
using SurveyBasket.API.DtoRequestAndResponse;

namespace SurveyBasket.API.FluentValidations
{
    public class ResendConfirmationEmailRequestValidator : AbstractValidator<ResendConfirmationEmailRequest>
    {
        public ResendConfirmationEmailRequestValidator()
        {
            RuleFor(x=>x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
