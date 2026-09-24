using ArticleService.Persistence.Contexts;
using ArticleService.Persistence.Repositories;

namespace ArticleService.Persistence;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .Validate(options =>
                !string.IsNullOrWhiteSpace(options.Africa.Host) &&
                !string.IsNullOrWhiteSpace(options.Antarctica.Host) &&
                !string.IsNullOrWhiteSpace(options.Asia.Host) &&
                !string.IsNullOrWhiteSpace(options.Europe.Host) &&
                !string.IsNullOrWhiteSpace(options.NorthAmerica.Host) &&
                !string.IsNullOrWhiteSpace(options.Oceania.Host) &&
                !string.IsNullOrWhiteSpace(options.SouthAmerica.Host) &&
                !string.IsNullOrWhiteSpace(options.Global.Host) &&
                !string.IsNullOrWhiteSpace(options.Name) &&
                !string.IsNullOrWhiteSpace(options.User) &&
                !string.IsNullOrWhiteSpace(options.Password),
                "Database configuration is incomplete.")
            .ValidateOnStart();

        services.AddSingleton<IArticleDbContextFactory, ArticleDbContextFactory>();
        services.AddScoped<IArticleRepository, ArticleRepository>();

        return services;
    }
}
