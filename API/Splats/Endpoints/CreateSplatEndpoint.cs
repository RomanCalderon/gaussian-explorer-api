using Domain.Splats;
using Domain.Splats.Requests;
using FastEndpoints;

namespace API.Splats.Endpoints;

public class CreateSplatEndpoint : Endpoint<CreateSplatRequest>
{
    private readonly ISplatsService _splatService;

    public CreateSplatEndpoint(ISplatsService splatService)
    {
        _splatService = splatService;
    }

    public override void Configure()
    {
        Post("/api/splats");
        AllowAnonymous();
        Throttle(
            hitLimit: 60,
            durationSeconds: 60,
            headerName: "X-Client-Id"
        );
        Description(b => b
            .WithName("CreateSplat")
            .WithTags("Splats"));
        Version(1);
    }

    public override async Task HandleAsync(CreateSplatRequest request, CancellationToken ct)
    {
        var splat = new Splat
        {
            UserId = request.UserId,
            Title = request.Title,
            Description = request.Description,
            Url = request.Url,
            ViewInfo = request.ViewInfo,
            CreatedAt = DateTime.UtcNow,
        };

        var createResult = await _splatService.AddAsync(splat, ct);
        if (createResult.IsFailure)
        {
            await SendResultAsync(Results.Problem(new()
            {
                Detail = createResult.Error.ToString(),
                Status = StatusCodes.Status500InternalServerError
            }));
            return;
        }

        await SendCreatedAtAsync<GetSplatEndpoint>(createResult.Value.Id, createResult.Value, cancellation: ct);
    }
}
