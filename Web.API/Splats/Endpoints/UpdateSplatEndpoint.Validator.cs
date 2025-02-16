using FastEndpoints;
using FluentValidation;
using Domain.Splats.Requests;

namespace API.Splats.Endpoints;

public class UpdateSplatValidator : Validator<UpdateSplatRequest>
{
    public UpdateSplatValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .When(x => x.Title is not null);

        RuleFor(x => x.Description)
            .NotEmpty()
            .When(x => x.Description is not null);

        RuleFor(x => x.Url)
            .NotEmpty()
            .When(x => x.Url is not null);

        RuleFor(x => x.ViewInfo)
            .NotEmpty()
            .When(x => x.ViewInfo is not null);
    }
}
