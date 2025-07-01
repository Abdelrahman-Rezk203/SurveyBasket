using FluentValidation;
using SurveyBasket.API.Practice_On_Mapster_WithoutProject;

namespace SurveyBasket.API.CustomAttributies
{
    public class StudentDtoRequestValidatiorCustomAttributies : AbstractValidator<StudentDtoRequest>
    {
        public StudentDtoRequestValidatiorCustomAttributies()
        {
            RuleFor(x => x.DateOFBirth)
                .Must(BeMoreThan18Years)
                .When(Condition => Condition.DateOFBirth.HasValue)
                .WithMessage("{PropertyName} Is Invalid , age should be 18 years");
            
        }

        private bool BeMoreThan18Years(DateTime? dateOfBirth)
        {
            return DateTime.Today > dateOfBirth!.Value.AddYears(18);
        }


    }
}
