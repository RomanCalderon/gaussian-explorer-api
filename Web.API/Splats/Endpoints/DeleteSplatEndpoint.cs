using Domain.Splats;
using FastEndpoints;
using GaussianExplorer.API.Splats;

namespace Web.API.Splats.Endpoints;

public class DeleteSplatEndpoint : EndpointWithoutRequest
{
    private readonly ISplatsService _splatsService;

    public DeleteSplatEndpoint(ISplatsService splatsService)
    {
        _splatsService = splatsService;
    }

    public override void Configure()
    {
        Delete("/api/splats/{id}");
        AllowAnonymous();
        Description(b => b
            .WithName("DeleteSplat")
            .WithTags("Splats"));
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        int id = Route<int>("id", isRequired: true);
        if (id < 1)
        {
            AddError(SplatErrors.InvalidID.Description!, SplatErrors.InvalidID.Code);
        }

        ThrowIfAnyErrors();

        var deleteResult = await _splatsService.RemoveAsync(id, ct);
        if (deleteResult.IsFailure)
        {
            await SendResultAsync(Results.NotFound(deleteResult.Error));
            return;
        }

        await SendResultAsync(Results.NoContent());
    }
}
