using Domain.Posts;
using Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Posts;

public sealed class PostsRepository : IPostsRepository
{
    private readonly ApplicationDbContext _context;

    public PostsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Post?> GetByIdAsync(int id, CancellationToken ct)
    {
        var post = await _context.Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);

        return post;
    }

    public async Task<PostCollection> GetByUserIdAsync(int id, CancellationToken ct)
    {
        var posts = await _context.Posts
            .AsNoTracking()
            .Where(p => p.UserId == id)
            .ToArrayAsync(ct);

        return new PostCollection(posts);
    }

    public async Task<PostCollection> GetPostsAsync(int page, int pageSize, CancellationToken ct)
    {
        var pagedPosts = await _context.Posts
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PostCollection(pagedPosts);
    }

    public async Task AddAsync(Post post, CancellationToken ct)
    {
        await _context.Posts.AddAsync(post, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Post post, CancellationToken ct)
    {
        _context.Update(post);
        await _context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(Post post, CancellationToken ct)
    {
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct)
    {
        return await _context.Posts.AnyAsync(p => p.Id == id, ct);
    }
}
