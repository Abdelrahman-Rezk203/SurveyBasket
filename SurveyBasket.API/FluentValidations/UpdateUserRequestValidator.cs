using FluentValidation;
using SurveyBasket.API.DtoRequestAndResponse.Users;

namespace SurveyBasket.API.FluentValidations
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(3, 60);


            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(3, 60);


            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();


            RuleFor(x => x.UserName)
                .NotEmpty()
                .Length(3, 60);


            RuleFor(x => x.Roles)
                .NotNull()
                .NotEmpty();


            RuleFor(x => x.Roles)
               .Must(x => x.Distinct().Count() == x.Count) // العدد من غير تكرار يساوي العدد بالتكرار 
               .WithMessage("You cannot add duplicated Roles for the same User")
               .When(x => x.Roles != null);


        }
    }
}
