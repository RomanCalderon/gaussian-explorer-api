using FastEndpoints;
using FastEndpoints.Swagger;
using GaussianExplorer.API;
using GaussianExplorer.API.RequestPipeline;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddCorsPolicy(builder.Configuration)
    .AddEndpoints()
    .AddPersistence(builder.Environment, builder.Configuration)
    .AddPostsService();

var app = builder.Build();

app.UseGlobalErrorHandling();
app.UseCors("default");
app.UseMiddleware<RequestLogContextMiddleware>();
app.UseSerilogRequestLogging();
app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();
