using CommentService.Persistence.Contexts;
using CommentService.Persistence.Repositories;
using CommentService.Repositories;

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

        services.AddScoped<ICommentDbContext, CommentDbContext>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        return services;
    }
}
