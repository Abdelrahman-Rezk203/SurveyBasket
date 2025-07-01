using FluentValidation;
using SurveyBasket.API.DtoRequestAndResponse;

namespace SurveyBasket.API.FluentValidations
{
    public class RegisterRequestValidation : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidation()
        {
            RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress();


            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

            RuleFor(y => y.FirstName)
                 .NotEmpty()
                 .Length(3, 80);


            RuleFor(y => y.LastName)
                 .NotEmpty()
                 .Length(3, 80);


                    
        }
    }
}
