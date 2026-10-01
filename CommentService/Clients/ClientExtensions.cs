using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;

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

        services
            .AddHttpClient<IProfanityClient, ProfanityClient>(
                (sp, client) =>
                {
                    var options = sp
                        .GetRequiredService<IOptions<ProfanityClientOptions>>()
                        .Value;

                    client.BaseAddress = new Uri(options.BaseUrl);
                })
            // Circuit breaker: https://learn.microsoft.com/en-us/dotnet/core/resilience/http-resilience
            .AddResilienceHandler("profanity-circuit-breaker", static builder =>
             {
                builder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    SamplingDuration = TimeSpan.FromSeconds(10),
                    FailureRatio = 0.5,
                    MinimumThroughput = 3,
                    BreakDuration = TimeSpan.FromSeconds(30)
                });
             });

        return services;
    }
}