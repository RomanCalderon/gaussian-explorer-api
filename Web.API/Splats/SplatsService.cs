using Domain.Splats;
using GaussianExplorer.Domain.Abstractions;
using GaussianExplorer.Domain.Splats.Requests;

namespace GaussianExplorer.API.Splats;

public sealed class SplatsService : ISplatsService
{
    private readonly ISplatsRepository _splatsRepository;

    public SplatsService(ISplatsRepository splatsRepository)
    {
        _splatsRepository = splatsRepository;
    }

    public async Task<Result<Splat>> GetAsync(int id, CancellationToken ct)
    {
        var splat = await _splatsRepository.GetAsync(id, ct);

        if (splat is null)
        {
            return Result<Splat>.Failure(SplatErrors.NotFound);
        }

        return Result<Splat>.Success(splat);
    }

    public async Task<Result<SplatCollection>> GetByUserIdAsync(int userId, CancellationToken ct)
    {
        var splats = await _splatsRepository.GetByUserIdAsync(userId, ct);

        return Result<SplatCollection>.Success(new SplatCollection(splats));
    }

    public async Task<Result<Splat>> AddAsync(Splat splat, CancellationToken ct)
    {
        if (splat is null)
        {
            return Result<Splat>.Failure(SplatErrors.InvalidSplat);
        }

        await _splatsRepository.AddAsync(splat, ct);
        return Result<Splat>.Success(splat);
    }

    public async Task<Result<Splat>> UpdateAsync(int id, UpdateSplatRequest request, CancellationToken ct)
    {
        var splat = await _splatsRepository.GetAsync(id, ct);
        if (splat is null)
        {
            return Result<Splat>.Failure(SplatErrors.NotFound);
        }

        var updatedSplat = new Splat
        {
            Id = id,
            UserId = splat.UserId,
            Title = request.Title ?? splat.Title,
            Description = request.Description ?? splat.Description,
            Url = request.Url ?? splat.Url,
            ViewInfo = request.ViewInfo ?? splat.ViewInfo,
            CreatedAt = splat.CreatedAt
        };

        await _splatsRepository.UpdateAsync(updatedSplat, ct);
        return Result<Splat>.Success(updatedSplat);
    }

    public async Task<Result> RemoveAsync(int id, CancellationToken ct)
    {
        var splat = await _splatsRepository.GetAsync(id, ct);
        if (splat is null)
        {
            return Result.Failure(SplatErrors.NotFound);
        }

        await _splatsRepository.RemoveAsync(splat, ct);
        return Result.Success();
    }
}
