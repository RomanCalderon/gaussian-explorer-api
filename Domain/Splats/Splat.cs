namespace Domain.Splats;

public sealed class Splat
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? ViewInfo { get; set; }
    public DateTime CreatedAt { get; set; }
}
