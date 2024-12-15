namespace GaussianExplorer.Domain.Posts;

public interface IPostsRepository
{
    Task<Post?> GetByIdAsync(int id, CancellationToken ct);

    Task<PostCollection> GetByUserIdAsync(int id, CancellationToken ct);

    Task<PostCollection> GetPostsAsync(int page, int pageSize, CancellationToken ct);

    Task AddAsync(Post post, CancellationToken ct);

    Task UpdateAsync(Post post, CancellationToken ct);

    Task RemoveAsync(Post post, CancellationToken ct);

    Task<bool> ExistsAsync(int id, CancellationToken ct);
}
