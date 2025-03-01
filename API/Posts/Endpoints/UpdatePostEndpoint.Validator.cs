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
        RuleFor(x => x.Summary).NotEmpty()
            .When(x => x.Summary is not null)
            .MaximumLength(128)
            .WithMessage("Summary can't be longer than 128 characters");
    }
}
