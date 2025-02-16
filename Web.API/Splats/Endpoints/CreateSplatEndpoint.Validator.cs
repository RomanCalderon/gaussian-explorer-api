using Domain.Splats.Requests;
using FastEndpoints;
using FluentValidation;

namespace API.Splats.Endpoints;

public class CreateSplatValidator : Validator<CreateSplatRequest>
{
    public CreateSplatValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Url).NotEmpty();
    }
}
