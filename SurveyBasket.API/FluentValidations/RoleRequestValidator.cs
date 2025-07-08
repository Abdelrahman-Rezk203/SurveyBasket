using FluentValidation;
using SurveyBasket.API.DtoRequestAndResponse.Roles;

namespace SurveyBasket.API.FluentValidations
{
    public class RoleRequestValidator : AbstractValidator<RoleRequest>
    {
        public RoleRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(3,50);

            RuleFor(x => x.Permissions)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Permissions)
                .Must(x => x.Distinct().Count() == x.Count) // العدد من غير تكرار يساوي العدد بالتكرار 
                .WithMessage("You cannot add duplicated permissions for the same role")
                .When(x => x.Permissions != null);

        }
    }
}
