using GaussianExplorer.Domain.Abstractions;

namespace GaussianExplorer.API.Splats;

public class SplatErrors
{
    public static readonly Error NotFound = new("Splats.NotFound", "The splat or splats could not be found");

    public static readonly Error InvalidSplat = new("Splats.InvalidSplat", "Splat is invalid/null");

    public static readonly Error InvalidID = new("Splats.InvalidID", "Invalid splat ID");

    public static readonly Error InvalidUserID = new("Splats.InvalidUserID", "Invalid user ID");
}
