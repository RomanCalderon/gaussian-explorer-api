namespace Domain.Splats.Requests;

public sealed class UpdateSplatRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? ViewInfo { get; set; }
}
