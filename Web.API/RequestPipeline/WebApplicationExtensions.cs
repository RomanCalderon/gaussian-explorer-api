using Microsoft.AspNetCore.Diagnostics;

namespace API.RequestPipeline;

public static class WebApplicationExtensions
{
    public static WebApplication UseGlobalErrorHandling(this WebApplication app)
    {
        app.UseExceptionHandler("/error");

        app.Map("/error", (HttpContext httpContext) =>
        {
            var exception = httpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;

            if (exception is null)
            {
                // Unexpected case
                return Results.Problem();
            }

            // Custom global error handling logic
            return exception switch
            {
                Exception ex => Results.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    detail: ex.Message),
                _ => Results.Problem()
            };
        });

        return app;
    }
}
