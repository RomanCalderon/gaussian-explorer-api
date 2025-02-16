using Domain.Abstractions;

namespace API.Posts;

public class PostErrors
{
    public static readonly Error NotFound = new("Posts.NotFound", "The post or posts could not be found");

    public static readonly Error InvalidPost = new("Posts.InvalidPost", "Post is invalid/null");

    public static readonly Error InvalidPage = new("Posts.InvalidPage", "Page can't be less than 1");
    public static readonly Error InvalidPageSize = new("Posts.InvalidPageSize", "Page Size can't be less than 1");

    public static readonly Error InvalidID = new("Posts.InvalidID", "Invalid post ID");
}
