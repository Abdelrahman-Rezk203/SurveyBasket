using FluentValidation;

namespace SurveyBasket.API.DtoRequestAndResponse.Common
{
    public class RequestFilterValidator : AbstractValidator<RequestFilter>
    {
        public RequestFilterValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);

        }

    }
}
