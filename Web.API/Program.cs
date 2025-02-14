using FastEndpoints;
using FastEndpoints.Swagger;
using GaussianExplorer.API;
using GaussianExplorer.API.Extensions;
using GaussianExplorer.API.RequestPipeline;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddCorsPolicy(builder.Configuration)
    .AddEndpoints()
    .AddPersistence(builder.Environment, builder.Configuration)
    .AddPostsService()
    .AddSplatsService();

var app = builder.Build();

await app.InitializeDatabaseAsync(app.Environment);

app.UseGlobalErrorHandling();
app.UseCors("default");
app.UseMiddleware<RequestLogContextMiddleware>();
app.UseSerilogRequestLogging();
app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();
