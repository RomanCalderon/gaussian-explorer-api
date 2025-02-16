using FastEndpoints;
using FluentValidation;
using Domain.Posts.Requests;

namespace API.Posts.Endpoints;

public class CreatePostValidator : Validator<CreatePostRequest>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();
    }
}
