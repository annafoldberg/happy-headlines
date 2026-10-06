using StackExchange.Redis;

namespace CommentService.Caching;

public static class CachingExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<CacheOptions>()
            .Bind(configuration.GetSection(CacheOptions.SectionName))
            .Validate(
                options =>
                    !string.IsNullOrWhiteSpace(options.Host) &&
                    options.Port > 0,
                "Comment cache configuration is incomplete.")
            .ValidateOnStart();

        var cacheOptions = configuration
            .GetSection(CacheOptions.SectionName)
                .Get<CacheOptions>()
                    ?? throw new InvalidOperationException(
                        "Comment cache configuration is missing.");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"{cacheOptions.Host}:{cacheOptions.Port}";
        });

        services
            .AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer
            .Connect($"{cacheOptions.Host}:{cacheOptions.Port},abortConnect=false"));

        services.AddScoped<ICommentCache, CommentCache>();

        return services;
    }
}