using FluentValidation;
using WebAggregator.Boundary.Request;

namespace WebAggregator.Infrastructure.Validators;

public class WebPageCreateRequestModelValidator : AbstractValidator<WebPageCreateRequestModel>
{
    public WebPageCreateRequestModelValidator()
    {
        RuleFor(r => r.Url)
            .NotEmpty().WithMessage("{PropertyName} is a required field.")
            .Length(0, 100).WithMessage("Max length for {PropertyName} is 100 characters.");
    }
}
