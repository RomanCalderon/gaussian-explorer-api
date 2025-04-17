using Domain.Splats;
using FastEndpoints;
using FastEndpoints.Swagger;
using API.Posts;
using API.Splats;
using Domain.Posts;
using Persistence.Data;
using Persistence.Posts;
using Persistence.Utility;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var corsConfig = configuration.GetSection("Cors");
        var origins = corsConfig.GetSection("AllowedOrigins").Get<string[]>()
            ?? throw new InvalidOperationException("Allowed origins are not configured");
        var allowAnyOrigin = corsConfig.GetValue<bool>("AllowAnyOrigin") && environment.IsDevelopment();

        services.AddCors(options =>
        {
            options.AddPolicy("development", builder =>
        {
            if (allowAnyOrigin)
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }
            else
            {
                builder.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }
        });

            options.AddPolicy("production", builder =>
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

    public static IServiceCollection AddSplatsService(this IServiceCollection services)
    {
        services.AddScoped<ISplatsRepository, SplatsRepository>();
        services.AddScoped<ISplatsService, SplatsService>();

        return services;
    }
}
