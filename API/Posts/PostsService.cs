using Domain.Abstractions;
using Domain.Posts;
using Domain.Posts.Requests;

namespace API.Posts;

public class PostsService : IPostsService
{
    private readonly IPostsRepository _postsRepository;

    public PostsService(IPostsRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }

    public async Task<Result<PostCollection>> GetPostsAsync(int page, int pageSize, CancellationToken ct)
    {
        var posts = await _postsRepository.GetPostsAsync(page, pageSize, ct);

        if (posts is null)
        {
            return Result<PostCollection>.Failure(PostErrors.NotFound);
        }

        return Result<PostCollection>.Success(new PostCollection(posts));
    }

    public async Task<Result<Post>> GetPostAsync(int id, CancellationToken ct)
    {
        var post = await _postsRepository.GetByIdAsync(id, ct);

        if (post is null)
        {
            return Result<Post>.Failure(PostErrors.NotFound);
        }

        return Result<Post>.Success(post);
    }

    public async Task<Result<Post>> CreatePostAsync(Post post, CancellationToken ct)
    {
        if (post is null)
        {
            return Result<Post>.Failure(PostErrors.InvalidPost);
        }

        await _postsRepository.AddAsync(post, ct);
        return Result<Post>.Success(post);
    }

    public async Task<Result<Post>> UpdatePostAsync(int id, UpdatePostRequest request, CancellationToken ct)
    {
        var post = await _postsRepository.GetByIdAsync(id, ct);

        if (post is null)
        {
            return Result<Post>.Failure(PostErrors.NotFound);
        }

        var updatedPost = new Post
        {
            Id = id,
            UserId = post.UserId,
            Title = request.Title ?? post.Title,
            Body = request.Body ?? post.Body,
            Summary = request.Summary ?? post.Summary,
            UpdatedAt = DateTime.UtcNow
        };

        await _postsRepository.UpdateAsync(updatedPost, ct);
        return Result<Post>.Success(updatedPost);
    }

    public async Task<Result> DeletePostAsync(int id, CancellationToken ct)
    {
        var post = await _postsRepository.GetByIdAsync(id, ct);

        if (post is null)
        {
            return Result.Failure(PostErrors.NotFound);
        }

        await _postsRepository.RemoveAsync(post, ct);
        return Result.Success();
    }
}
