using CommentService.Persistence.Contexts;
using CommentService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CommentService.Persistence;

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

        services.AddDbContext<CommentDbContext>((serviceProvider, options) =>
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

        services.AddScoped<ICommentDbContext>(
            provider => provider.GetRequiredService<CommentDbContext>());


        services.AddScoped<ICommentRepository, CommentRepository>();

        return services;
    }
}
