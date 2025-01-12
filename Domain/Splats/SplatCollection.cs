using System.Collections;

namespace Domain.Splats;

public sealed record SplatCollection(IEnumerable<Splat> Splats) : IReadOnlyCollection<Splat>
{
    private readonly List<Splat> _splats = new(Splats);

    public int Count => _splats.Count;

    public IEnumerator<Splat> GetEnumerator() => _splats.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
