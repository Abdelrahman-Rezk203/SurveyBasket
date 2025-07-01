using FluentValidation;
using SurveyBasket.API.DtoRequestAndResponse;

namespace SurveyBasket.API.FluentValidations
{
    public class ForgetPasswordRequestValidator : AbstractValidator<ForgetPasswordRequest>
    {
        public ForgetPasswordRequestValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        
        }
    }
}
