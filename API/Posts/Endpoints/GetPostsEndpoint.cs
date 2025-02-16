using FastEndpoints;
using Domain.Abstractions;
using Domain.Posts;

namespace API.Posts.Endpoints;

public class GetPostsEndpoint : EndpointWithoutRequest
{
    private readonly IPostsService _postService;

    public GetPostsEndpoint(IPostsService postService)
    {
        _postService = postService;
    }

    public override void Configure()
    {
        Get("/api/posts");
        AllowAnonymous();
        Description(b => b
            .WithName("GetPosts")
            .WithTags("Posts"));
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var page = Query<int>("page", isRequired: true);
        var pageSize = Query<int>("pageSize", isRequired: true);

        if (page < 1)
        {
            await SendResultAsync(Results.BadRequest(Result<PostCollection>.Failure(PostErrors.InvalidPage)));
            return;
        }

        if (pageSize < 1)
        {
            await SendResultAsync(Results.BadRequest(Result<PostCollection>.Failure(PostErrors.InvalidPageSize)));
            return;
        }

        var postsResult = await _postService.GetPostsAsync(page, pageSize, ct);

        if (postsResult.IsFailure)
        {
            await SendResultAsync(Results.Problem(new()
            {
                Detail = postsResult.Error.ToString(),
                Status = 500
            }));
            return;
        }

        await SendResultAsync(Results.Ok(postsResult.Value));
    }
}
