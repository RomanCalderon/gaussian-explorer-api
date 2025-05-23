﻿namespace Domain.Posts.Requests;

public sealed class CreatePostRequest
{
    public int UserId { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public string? Summary { get; set; }
}
