using Domain.Posts;
using Domain.Posts.Requests;
using FastEndpoints;

namespace API.Posts.Endpoints;

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
        Throttle(
            hitLimit: 120,
            durationSeconds: 60,
            headerName: "X-Client-Id"
        );
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
            Body = request.Body,
            Summary = request.Summary ?? string.Empty,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
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
