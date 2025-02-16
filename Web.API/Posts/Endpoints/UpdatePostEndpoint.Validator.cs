using FastEndpoints;
using FluentValidation;
using Domain.Posts.Requests;

namespace API.Posts.Endpoints;

public class UpdatePostValidator : Validator<UpdatePostRequest>
{
    public UpdatePostValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.Body).NotEmpty().WithMessage("Body is required");
    }
}
