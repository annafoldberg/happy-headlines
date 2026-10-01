using Microsoft.Extensions.Options;

namespace NewsletterService.Clients;

public static class ClientExtensions
{
    public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<ArticleClientOptions>()
            .Bind(configuration.GetSection(ArticleClientOptions.SectionName))
            .Validate(options =>
                !string.IsNullOrWhiteSpace(options.BaseUrl),
                "Article client configuration is incomplete.")
            .ValidateOnStart();

        services.AddHttpClient<IArticleClient, ArticleClient>(
            (sp, client) =>
            {
                var options = sp
                    .GetRequiredService<IOptions<ArticleClientOptions>>()
                    .Value;

                client.BaseAddress = new Uri(options.BaseUrl);
            });

        return services;
    }
}