using API.UnitTests.Common;
using GaussianExplorer.API.Posts;
using GaussianExplorer.Domain.Abstractions;
using GaussianExplorer.Domain.Posts;
using System.Collections;

namespace API.UnitTests.Posts.Fixtures;

public class CreatePostTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Success cases
        yield return new InlineTestArgs<Post, Result<Post>>[]
        {
            new() {
                InputValue = new Post
                {
                    UserId = 1,
                    Title = "Title",
                    Body = "Body"
                },
                ExpectedResult = Result<Post>.Success(new Post
                {
                    UserId = 1,
                    Title = "Title 2",
                    Body = "Body 2"
                })
            }
        };

        yield return new InlineTestArgs<Post, Result<Post>>[]
        {
            new() {
                InputValue = new Post
                {
                    UserId = 2,
                    Title = "Title 2",
                    Body = "Body 2"
                },
                ExpectedResult = Result<Post>.Success(new Post
                {
                    UserId = 2,
                    Title = "Title 2",
                    Body = "Body 2"
                })
            }
        };

        // Failure cases
        yield return new InlineTestArgs<Post, Result<Post>>[]
        {
            new() {
                InputValue = null,
                ExpectedResult = Result<Post>.Failure(PostErrors.InvalidPost)
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
