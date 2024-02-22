using FluentValidation;
using WebAggregator.Boundary.Request;

namespace WebAggregator.Infrastructure.Validators;

public class WebPageCreateRequestModelValidator : AbstractValidator<WebPageCreateRequestModel>
{
    public WebPageCreateRequestModelValidator()
    {
        RuleFor(r => r.Title)
            .NotEmpty().WithMessage("{PropertyName} is a required field.")
            .Length(0, 10).WithMessage("Max length for {PropertyName} is 100 characters.");

        RuleFor(r => r.Content)
            .NotEmpty().WithMessage("{PropertyName} is a required field.")
            .Length(0, 10000).WithMessage("Max length for {PropertyName} is 60 characters.");

        RuleFor(r => r.Url)
            .NotEmpty().WithMessage("{PropertyName} is a required field.")
            .Length(0, 100).WithMessage("Max length for {PropertyName} is 100 characters.");
    }
}
