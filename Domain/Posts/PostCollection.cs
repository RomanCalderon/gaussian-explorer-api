using System.Collections;

namespace GaussianExplorer.Domain.Posts;

public sealed class PostCollection : IReadOnlyCollection<Post>
{
    private readonly List<Post> _posts;

    public PostCollection(IEnumerable<Post> posts)
    {
        _posts = new(posts);
    }

    #region IReadOnlyCollection implementations
    public int Count => _posts.Count;

    public IEnumerator<Post> GetEnumerator() => _posts.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}
