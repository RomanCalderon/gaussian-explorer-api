using FastEndpoints;
using GaussianExplorer.API.Posts;
using GaussianExplorer.Domain.Posts;

namespace Web.API.Posts.Endpoints;

public class DeletePostEndpoint : EndpointWithoutRequest
{
    private readonly IPostsService _postService;

    public DeletePostEndpoint(IPostsService postService)
    {
        _postService = postService;
    }

    public override void Configure()
    {
        Delete("/api/posts/{id}");
        AllowAnonymous();
        Description(b => b
            .WithName("DeletePost")
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

        var deleteResult = await _postService.DeletePostAsync(id, ct);

        if (deleteResult.IsFailure)
        {
            await SendResultAsync(Results.NotFound(deleteResult.Error));
            return;
        }

        await SendResultAsync(Results.NoContent());
    }
}
