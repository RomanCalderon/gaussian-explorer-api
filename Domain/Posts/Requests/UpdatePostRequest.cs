namespace Domain.Posts.Requests;

public sealed class UpdatePostRequest
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public string? Summary { get; set; }
}
