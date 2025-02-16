using Domain.Abstractions;
using Domain.Posts.Requests;

namespace Domain.Posts;

public interface IPostsService
{
    Task<Result<PostCollection>> GetPostsAsync(int page, int pageSize, CancellationToken ct);
    Task<Result<Post>> GetPostAsync(int id, CancellationToken ct);
    Task<Result<Post>> CreatePostAsync(Post post, CancellationToken ct);
    Task<Result<Post>> UpdatePostAsync(int id, UpdatePostRequest request, CancellationToken ct);
    Task<Result> DeletePostAsync(int id, CancellationToken ct);
}
