using Domain.Splats.Requests;
using FastEndpoints;
using FluentValidation;

namespace Web.API.Splats.Endpoints;

public class CreateSplatEndpoint : Validator<CreateSplatRequest>
{
    public CreateSplatEndpoint()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Url).NotEmpty();
    }
}
