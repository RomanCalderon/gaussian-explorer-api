﻿using Domain.Posts;
using Domain.Posts.Requests;
using FastEndpoints;
using FluentValidation;

namespace API.Posts.Endpoints;

public class UpdatePostEndpoint : Endpoint<UpdatePostRequest>
{
    private readonly IPostsService _postService;

    public UpdatePostEndpoint(IPostsService postService)
    {
        _postService = postService;
    }

    public override void Configure()
    {
        Put("/api/posts/{id}");
        AllowAnonymous();
        Throttle(
            hitLimit: 120,
            durationSeconds: 60,
            headerName: "X-Client-Id"
        );
        Description(b => b
            .WithName("UpdatePost")
            .WithTags("Posts"));
        Version(1);
    }

    public override async Task HandleAsync(UpdatePostRequest request, CancellationToken ct)
    {
        int id = Route<int>("id", isRequired: true);

        if (id < 1)
        {
            AddError("ID cannot be less than 1", PostErrors.InvalidID.Code);
        }

        ThrowIfAnyErrors();

        var postResult = await _postService.UpdatePostAsync(id, request, ct);

        if (postResult.IsFailure)
        {
            await SendResultAsync(Results.NotFound(postResult.Error));
            return;
        }

        await SendResultAsync(Results.Ok(postResult.Value));

    }
}
