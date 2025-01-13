using Domain.Splats;
using GaussianExplorer.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class SplatsRepository : ISplatsRepository
{
    private readonly ApplicationDbContext _context;

    public SplatsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Splat?> GetAsync(int id, CancellationToken ct)
    {
        var splat = await _context.Splats
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        return splat;
    }

    public async Task<SplatCollection> GetByUserIdAsync(int userId, CancellationToken ct)
    {
        var splats = await _context.Splats
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .ToArrayAsync(ct);

        return new SplatCollection(splats);
    }

    public async Task AddAsync(Splat splat, CancellationToken ct)
    {
        await _context.Splats.AddAsync(splat, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Splat splat, CancellationToken ct)
    {
        _context.Update(splat);
        await _context.SaveChangesAsync(ct);
    }

    public Task RemoveAsync(Splat splat, CancellationToken ct)
    {
        _context.Splats.Remove(splat);
        return _context.SaveChangesAsync(ct);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken ct)
    {
        return _context.Splats.AnyAsync(s => s.Id == id, ct);
    }
}
