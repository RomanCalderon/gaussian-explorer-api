﻿using API.Splats.Errors;
using Domain.Splats;
using Domain.Splats.Requests;
using FastEndpoints;

namespace API.Splats.Endpoints;

public class UpdateSplatEndpoint : Endpoint<UpdateSplatRequest>
{
    private readonly ISplatsService _splatsService;

    public UpdateSplatEndpoint(ISplatsService splatsService)
    {
        _splatsService = splatsService;
    }

    public override void Configure()
    {
        Put("/api/splats/{id}");
        AllowAnonymous();
        Throttle(
            hitLimit: 60,
            durationSeconds: 60,
            headerName: "X-Client-Id"
        );
        Description(b => b
            .WithName("UpdateSplat")
            .WithTags("Splats"));
        Version(1);
    }

    public override async Task HandleAsync(UpdateSplatRequest request, CancellationToken ct)
    {
        int id = Route<int>("id", isRequired: true);
        if (id < 1)
        {
            AddError(SplatErrors.InvalidID.Description!, SplatErrors.InvalidID.Code);
        }

        ThrowIfAnyErrors();

        var updateResult = await _splatsService.UpdateAsync(id, request, ct);
        if (updateResult.IsFailure)
        {
            await SendResultAsync(Results.UnprocessableEntity(updateResult.Error));
            return;
        }

        await SendResultAsync(Results.Ok(updateResult.Value));
    }
}
