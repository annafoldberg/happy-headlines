using Microsoft.Extensions.Options;

namespace CommentService.Clients;

public static class ClientExtensions
{
    public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<ProfanityClientOptions>()
            .Bind(configuration.GetSection(ProfanityClientOptions.SectionName))
            .Validate(options =>
                !string.IsNullOrWhiteSpace(options.BaseUrl),
                "Profanity client configuration is incomplete.")
            .ValidateOnStart();

        services.AddHttpClient<IProfanityClient, ProfanityClient>(
            (sp, client) =>
            {
                var options = sp
                    .GetRequiredService<IOptions<ProfanityClientOptions>>()
                    .Value;

                client.BaseAddress = new Uri(options.BaseUrl);
            });

        return services;
    }
}