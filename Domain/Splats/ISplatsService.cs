using GaussianExplorer.Domain.Abstractions;
using GaussianExplorer.Domain.Splats.Requests;

namespace Domain.Splats;

public interface ISplatsService
{
    Task<Result<Splat>> GetAsync(int id, CancellationToken ct);
    Task<Result<SplatCollection>> GetByUserIdAsync(int userId, CancellationToken ct);
    Task<Result<Splat>> AddAsync(Splat splat, CancellationToken ct);
    Task<Result<Splat>> UpdateAsync(int id, UpdateSplatRequest request, CancellationToken ct);
    Task<Result> RemoveAsync(int id, CancellationToken ct);
}
