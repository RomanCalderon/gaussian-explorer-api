using FastEndpoints;
using FluentValidation;
using GaussianExplorer.Domain.Splats.Requests;

namespace GaussianExplorer.API.Splats.Endpoints;

public class UpdateSplatEndpoint : Validator<UpdateSplatRequest>
{
    public UpdateSplatEndpoint()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Url).NotEmpty();
    }
}
