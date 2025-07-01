using FluentValidation;
using SurveyBasket.API.DtoRequestAndResponse;

namespace SurveyBasket.API.FluentValidations
{
    public class UserProfileRequestValidator : AbstractValidator<UserProfileRequest>
    {
        public UserProfileRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(x => x.UserName)
                .NotEmpty()
                .Length(3, 60)
                .Matches(@"^(?=.*\d)[A-Za-z\d]+$")
                .WithMessage("UserName must contain at least one number and no spaces.");


        }
    }

}
