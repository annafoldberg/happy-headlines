using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProfanityService.Persistence.Contexts;
using ProfanityService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ProfanityService.Persistence;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .Validate(options =>
                !string.IsNullOrWhiteSpace(options.Host) &&
                !string.IsNullOrWhiteSpace(options.Name) &&
                !string.IsNullOrWhiteSpace(options.User) &&
                !string.IsNullOrWhiteSpace(options.Password),
                "Database configuration is incomplete.")
            .ValidateOnStart();

        services.AddDbContext<ProfanityDbContext>((serviceProvider, options) =>
        {
            var databaseOptions = serviceProvider
                .GetRequiredService<IOptions<DatabaseOptions>>()
                .Value;

            var connectionString =
                $"Host={databaseOptions.Host};" +
                $"Database={databaseOptions.Name};" +
                $"Username={databaseOptions.User};" +
                $"Password={databaseOptions.Password}";

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IProfanityDbContext>(
            provider => provider.GetRequiredService<ProfanityDbContext>());
            
        services.AddScoped<IProfanityRepository, ProfanityRepository>();

        return services;
    }
}
