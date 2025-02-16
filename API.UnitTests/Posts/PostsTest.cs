using API.UnitTests.Common;
using API.UnitTests.Posts.Fixtures;
using API.Posts;
using Domain.Abstractions;
using Domain.Posts;
using Domain.Posts.Requests;
using Moq;

namespace API.UnitTests.Posts;

public class PostsTest
{
    private readonly Mock<IPostsRepository> _postsRepositoryMock;
    private readonly PostsService _postService;

    public PostsTest()
    {
        _postsRepositoryMock = new Mock<IPostsRepository>();
        _postService = new PostsService(_postsRepositoryMock.Object);
    }

    #region Get Methods
    [Fact]
    public async void GetPostsAsync_ShouldReturnPosts_RepositoryCallIsSuccessful()
    {
        // Arrange
        Post[] expectedPosts = TestDataGenerator.PostsArray;

        _postsRepositoryMock.Setup(repo => repo.GetPostsAsync(1, 10, CancellationToken.None))
            .ReturnsAsync(new PostCollection(expectedPosts));

        // Act
        Result<PostCollection> result = await _postService.GetPostsAsync(1, 10, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedPosts.Length, result.Value.Count);
        Assert.Equal(expectedPosts[0].Id, result.Value.First().Id);
        Assert.Equal(expectedPosts[0].Title, result.Value.First().Title);

        _postsRepositoryMock.Verify(
            repo => repo.GetPostsAsync(1, 10, CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async void GetPostsAsync_ShouldReturnNotFound_RepositoryCallIsUnsuccessful()
    {
        // Arrange
        PostCollection? nullCollection = null;

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _postsRepositoryMock.Setup(repo => repo.GetPostsAsync(1, 10, CancellationToken.None))
            .ReturnsAsync(nullCollection);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act
        Result<PostCollection> result = await _postService.GetPostsAsync(1, 10, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(PostErrors.NotFound, result.Error);

        _postsRepositoryMock.Verify(
            repo => repo.GetPostsAsync(1, 10, CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async void GetPostAsync_ShouldReturnPost_RepositoryCallIsSuccessful()
    {
        // Arrange
        Post expectedPost = TestDataGenerator.PostsArray[0];

        _postsRepositoryMock.Setup(repo => repo.GetByIdAsync(1, CancellationToken.None))
            .ReturnsAsync(expectedPost);

        // Act
        Result<Post> result = await _postService.GetPostAsync(1, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedPost.Id, result.Value.Id);
        Assert.Equal(expectedPost.Title, result.Value.Title);

        _postsRepositoryMock.Verify(
            repo => repo.GetByIdAsync(1, CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async void GetPostAsync_ShouldReturnNotFound_RepositoryCallIsUnsuccessful()
    {
        // Arrange
        Post? nullPost = null;

        _postsRepositoryMock.Setup(repo => repo.GetByIdAsync(1, CancellationToken.None))
            .ReturnsAsync(nullPost);

        // Act
        Result<Post> result = await _postService.GetPostAsync(1, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(PostErrors.NotFound, result.Error);

        _postsRepositoryMock.Verify(
            repo => repo.GetByIdAsync(1, CancellationToken.None),
            Times.Once);
    }
    #endregion

    [Theory]
    [ClassData(typeof(CreatePostTestData))]
    public async void CreatePostAsync(InlineTestArgs<Post, Result<Post>> testArgs)
    {
        // Arrange
        Post? post = testArgs.InputValue;

        // Act
        Result<Post> result = await _postService.CreatePostAsync(post!, CancellationToken.None);

        // Assert
        bool expectedSuccess = testArgs.ExpectedResult.IsSuccess;
        Assert.Equal(expectedSuccess, result.IsSuccess);

        if (expectedSuccess)
        {
            Assert.Equal(testArgs.InputValue!.UserId, result.Value.UserId);
            Assert.Equal(testArgs.InputValue!.Title, result.Value.Title);
            Assert.Equal(testArgs.InputValue!.Body, result.Value.Body);
        }
        else
        {
            Assert.Equal(testArgs.ExpectedResult.Error, result.Error);
            Assert.Null(result.Value);
        }

        _postsRepositoryMock.Verify(
            repo => repo.AddAsync(post!, CancellationToken.None),
            expectedSuccess ? Times.Once : Times.Never);
    }

    [Theory]
    [ClassData(typeof(UpdatePostTestData))]
    public async void UpdatePostAsync(InlineTestArgs<int, UpdatePostRequest, Result<Post>> testArgs)
    {
        // Arrange
        int inputId = testArgs.InputValue1;
        UpdatePostRequest inputUpdateRequest = testArgs.InputValue2!;
        Result<Post> expectedResult = testArgs.ExpectedResult;

        _postsRepositoryMock.Setup(repo => repo.GetByIdAsync(inputId, CancellationToken.None))
            .ReturnsAsync(expectedResult.Value);

        // Act
        Result<Post> result = await _postService.UpdatePostAsync(inputId, inputUpdateRequest, CancellationToken.None);

        // Assert
        bool expectedSuccess = testArgs.ExpectedResult.IsSuccess;
        Assert.Equal(expectedSuccess, result.IsSuccess);

        if (expectedSuccess)
        {
            Assert.Equal(inputId, result.Value.Id);
            Assert.Equal(inputUpdateRequest.Title, result.Value.Title);
            Assert.Equal(inputUpdateRequest.Body, result.Value.Body);
        }
        else
        {
            Assert.Equal(testArgs.ExpectedResult.Error, result.Error);
            Assert.Null(result.Value);
        }

        _postsRepositoryMock.Verify(
            repo => repo.GetByIdAsync(inputId, CancellationToken.None),
            Times.Once);

        _postsRepositoryMock.Verify(
            repo => repo.UpdateAsync(It.IsAny<Post>(), CancellationToken.None),
            expectedSuccess ? Times.Once : Times.Never);
    }
}
