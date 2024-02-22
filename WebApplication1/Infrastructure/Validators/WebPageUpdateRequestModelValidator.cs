using FluentValidation;
using WebAggregator.Boundary.Request;

namespace WebAggregator.Infrastructure.Validators;

public class WebPageUpdateRequestModelValidator : AbstractValidator<WebPageUpdateRequestModel>
{
    public WebPageUpdateRequestModelValidator()
    {
        RuleFor(r => r.Url)
            .NotEmpty().WithMessage("{PropertyName} is a required field.")
            .Length(0, 100).WithMessage("Max length for {PropertyName} is 100 characters.");
    }
}
