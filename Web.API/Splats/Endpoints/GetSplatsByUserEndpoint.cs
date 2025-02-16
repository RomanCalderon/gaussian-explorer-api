using Domain.Splats;
using FastEndpoints;
using API.Splats.Errors;

namespace API.Splats.Endpoints;

public class GetSplatsByUserEndpoint : EndpointWithoutRequest
{
    private readonly ISplatsService _splatService;

    public GetSplatsByUserEndpoint(ISplatsService splatService)
    {
        _splatService = splatService;
    }

    public override void Configure()
    {
        Get("/api/splats/user/{userId}");
        AllowAnonymous();
        Description(b => b
            .WithName("GetSplatsByUser")
            .WithTags("Splats"));
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        int userId = Route<int>("userId", isRequired: true);

        if (userId < 1)
        {
            AddError("User ID cannot be less than 1", SplatErrors.InvalidUserID.Code);
        }

        ThrowIfAnyErrors();

        var splatResult = await _splatService.GetByUserIdAsync(userId, ct);
        if (splatResult.IsFailure)
        {
            await SendResultAsync(Results.NotFound(splatResult.Error));
            return;
        }

        await SendResultAsync(Results.Ok(splatResult.Value));
    }
}
