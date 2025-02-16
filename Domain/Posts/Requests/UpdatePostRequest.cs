namespace Domain.Posts.Requests;

public sealed class UpdatePostRequest
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
}
