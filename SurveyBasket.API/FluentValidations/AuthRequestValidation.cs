using FluentValidation;
using SurveyBasket.API.Dto;

namespace SurveyBasket.API.FluentValidations
{
    public class AuthRequestValidation : AbstractValidator<AuthRequest>
    {
        public AuthRequestValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
                
        }
    }
}
