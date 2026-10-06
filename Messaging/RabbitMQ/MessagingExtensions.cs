using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Messaging.RabbitMQ;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<RabbitMQOptions>()
            .Bind(configuration.GetSection(RabbitMQOptions.SectionName))
            .Validate(options =>
                !string.IsNullOrWhiteSpace(options.Host) &&
                !string.IsNullOrWhiteSpace(options.User) &&
                !string.IsNullOrWhiteSpace(options.Password) &&
                options.Port > 0,
                "RabbitMQ configuration is incomplete.")
            .ValidateOnStart();

        services.AddSingleton<IEventPublisher>(sp =>
        {
            var options = sp
                .GetRequiredService<IOptions<RabbitMQOptions>>()
                .Value;

            return new RabbitMQPublisher(
                options.Host,
                options.User,
                options.Password,
                options.Port);
        });

        services.AddSingleton<IEventConsumer>(sp =>
        {
            var options = sp
                .GetRequiredService<IOptions<RabbitMQOptions>>()
                .Value;

            return new RabbitMQConsumer(
                options.Host,
                options.User,
                options.Password,
                options.Port);
        });

        return services;
    }
}