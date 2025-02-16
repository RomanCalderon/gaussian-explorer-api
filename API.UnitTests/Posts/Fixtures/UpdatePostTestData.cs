using API.UnitTests.Common;
using API.Posts;
using Domain.Abstractions;
using Domain.Posts;
using Domain.Posts.Requests;
using System.Collections;

namespace API.UnitTests.Posts.Fixtures;

public class UpdatePostTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Success cases
        yield return new InlineTestArgs<int, UpdatePostRequest, Result<Post>>[]
        {
            new()
            {
                InputValue1 = 1,
                InputValue2 = new UpdatePostRequest
                {
                    Title = "Title New",
                    Body = "Body"
                },
                ExpectedResult = Result<Post>.Success(new Post
                {
                    Id = 1,
                    Title = "Title New",
                    Body = "Body"
                })
            }
        };

        // Failure cases
        yield return new InlineTestArgs<int, UpdatePostRequest, Result<Post>>[]
        {
            new()
            {
                InputValue1 = 1,
                InputValue2 = new UpdatePostRequest
                {
                    Id = 2,
                    Title = "Title",
                    Body = "Body"
                },
                ExpectedResult = Result<Post>.Failure(PostErrors.NotFound)
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
