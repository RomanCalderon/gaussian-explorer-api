namespace Domain.Splats;

public interface ISplatsRepository
{
    Task<Splat?> GetAsync(int id, CancellationToken ct);
    Task<SplatCollection> GetByUserIdAsync(int userId, CancellationToken ct);
    Task AddAsync(Splat splat, CancellationToken ct);
    Task UpdateAsync(Splat splat, CancellationToken ct);
    Task RemoveAsync(Splat splat, CancellationToken ct);
}
