using FastEndpoints;
using FluentValidation;
using GaussianExplorer.Domain.Splats.Requests;

namespace GaussianExplorer.API.Splats.Endpoints;

public class UpdateSplatEndpoint : Validator<UpdateSplatRequest>
{
    public UpdateSplatEndpoint()
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
