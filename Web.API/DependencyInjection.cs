using FastEndpoints;
using FastEndpoints.Swagger;
using GaussianExplorer.API.Posts;
using GaussianExplorer.Domain.Posts;
using GaussianExplorer.Persistence.Data;
using GaussianExplorer.Persistence.Posts;
using GaussianExplorer.Persistence.Utility;
using Microsoft.EntityFrameworkCore;

namespace GaussianExplorer.API;

public static class DependencyInjection
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetRequiredSection("Cors:AllowedOrigins").Get<string[]>()
            ?? throw new InvalidOperationException("Allowed origins are not configured");

        services.AddCors(options =>
        {
            options.AddPolicy("default", builder =>
            {
                builder.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        services
            .AddFastEndpoints()
            .SwaggerDocument(o =>
            {
                o.DocumentSettings = s =>
                {
                    s.Title = "Gaussian Explorer API";
                    s.Description = "API for Gaussian Explorer";
                    s.Version = "v1";
                };
            });

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
    {
        var connectionString = environment.IsDevelopment()
            ? configuration.GetConnectionString("LocalDB")
            : Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");

        ArgumentException.ThrowIfNullOrEmpty(connectionString, nameof(connectionString));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, options =>
            {
                options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null
                );
            });
        });

        services.AddScoped<IDatabaseInitializer>(sp =>
        {
            var dbContext = sp.GetRequiredService<ApplicationDbContext>();
            var logger = sp.GetRequiredService<ILogger<DatabaseInitializer>>();
            return new DatabaseInitializer(dbContext, logger);
        });

        return services;
    }

    public static IServiceCollection AddPostsService(this IServiceCollection services)
    {
        services.AddScoped<IPostsRepository, PostsRepository>();
        services.AddScoped<IPostsService, PostsService>();

        return services;
    }
}
