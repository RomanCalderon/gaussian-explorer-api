using FastEndpoints;
using FastEndpoints.Swagger;
using API;
using API.Extensions;
using API.RequestPipeline;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddCorsPolicy(builder.Configuration, builder.Environment)
    .AddEndpoints()
    .AddPersistence(builder.Environment, builder.Configuration)
    .AddPostsService()
    .AddSplatsService();

var app = builder.Build();

await app.InitializeDatabaseAsync(app.Environment);

app.UseGlobalErrorHandling();
app.UseCors(app.Environment.IsDevelopment() ? "development" : "production");
app.UseMiddleware<RequestLogContextMiddleware>();
app.UseSerilogRequestLogging();
app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();
