using FluentValidation;
using SurveyBasket.API.DtoRequestAndResponse;

namespace SurveyBasket.API.FluentValidations
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator() 
        {
            RuleFor(x => x.CurrentPasword)
                .NotEmpty();


            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase")
                .NotEqual(x => x.CurrentPasword)
                .WithMessage("NewPassword Can't be Same as the CurrentPassword");
                

        }
    }
}
