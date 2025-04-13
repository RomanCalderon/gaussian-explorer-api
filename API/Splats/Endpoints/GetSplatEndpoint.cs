using API.Splats.Errors;
using Domain.Splats;
using FastEndpoints;
using API.Splats.Errors;

namespace API.Splats.Endpoints;

public class GetSplatEndpoint : EndpointWithoutRequest
{
    private readonly ISplatsService _splatService;

    public GetSplatEndpoint(ISplatsService splatService)
    {
        _splatService = splatService;
    }

    public override void Configure()
    {
        Get("/api/splats/{id}");
        AllowAnonymous();
        Throttle(
            hitLimit: 60,
            durationSeconds: 60,
            headerName: "X-Client-Id"
        );
        Description(b => b
            .WithName("GetSplat")
            .WithTags("Splats"));
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        int id = Route<int>("id", isRequired: true);
        if (id < 1)
        {
            AddError("ID cannot be less than 1", SplatErrors.InvalidID.Code);
        }

        ThrowIfAnyErrors();

        var splatResult = await _splatService.GetAsync(id, ct);
        if (splatResult.IsFailure)
        {
            await SendResultAsync(Results.NotFound(splatResult.Error));
            return;
        }

        await SendResultAsync(Results.Ok(splatResult.Value));
    }
}
