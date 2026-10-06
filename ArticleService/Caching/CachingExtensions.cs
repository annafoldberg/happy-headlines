namespace ArticleService.Caching;

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
                "Article cache configuration is incomplete.")
            .ValidateOnStart();

        var cacheOptions = configuration
            .GetSection(CacheOptions.SectionName)
                .Get<CacheOptions>()
                    ?? throw new InvalidOperationException(
                        "Article cache configuration is missing.");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"{cacheOptions.Host}:{cacheOptions.Port},abortConnect=false";
        });
        services.AddScoped<IArticleCache, ArticleCache>();

        return services;
    }
}