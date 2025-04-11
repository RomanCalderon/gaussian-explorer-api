using FastEndpoints;
using FluentValidation;
using Domain.Posts;

namespace API.Posts.Endpoints;

public class GetPostEndpoint : EndpointWithoutRequest
{
    private readonly IPostsService _postService;

    public GetPostEndpoint(IPostsService postService)
    {
        _postService = postService;
    }

    public override void Configure()
    {
        Get("/api/posts/{id}");
        AllowAnonymous();
        Throttle(
            hitLimit: 120,
            durationSeconds: 60,
            headerName: "X-Client-Id"
        );
        Description(b => b
            .WithName("GetPost")
            .WithTags("Posts"));
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        int id = Route<int>("id", isRequired: true);

        if (id < 1)
        {
            AddError("ID cannot be less than 1", PostErrors.InvalidID.Code);
        }

        ThrowIfAnyErrors();

        var postResult = await _postService.GetPostAsync(id, ct);

        if (postResult.IsFailure)
        {
            await SendResultAsync(Results.NotFound(postResult.Error));
            return;
        }

        await SendResultAsync(Results.Ok(postResult.Value));
    }
}
