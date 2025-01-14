namespace Domain.Splats.Requests;

public sealed class CreateSplatRequest
{
    public int UserId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Url { get; set; }
    public string? ViewInfo { get; set; }
}
