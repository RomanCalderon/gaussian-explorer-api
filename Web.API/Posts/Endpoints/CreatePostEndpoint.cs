using FastEndpoints;
using GaussianExplorer.Domain.Posts;
using GaussianExplorer.Domain.Posts.Requests;

namespace GaussianExplorer.API.Posts.Endpoints;

public class CreatePostEndpoint : Endpoint<CreatePostRequest>
{
    private readonly IPostsService _postService;

    public CreatePostEndpoint(IPostsService postService)
    {
        _postService = postService;
    }

    public override void Configure()
    {
        Post("/api/posts");
        AllowAnonymous();
        Description(b => b
            .WithName("CreatePost")
            .WithTags("Posts"));
        Version(1);
    }

    public override async Task HandleAsync(CreatePostRequest request, CancellationToken ct)
    {
        var post = new Post
        {
            UserId = request.UserId,
            Title = request.Title,
            Body = request.Body
        };
        var createResult = await _postService.CreatePostAsync(post, ct);

        if (createResult.IsFailure)
        {
            await SendResultAsync(Results.Problem(new()
            {
                Detail = createResult.Error.ToString(),
                Status = StatusCodes.Status500InternalServerError
            }));
            return;
        }

        await SendCreatedAtAsync<GetPostEndpoint>(createResult.Value.Id, createResult.Value, cancellation: ct);
    }
}
