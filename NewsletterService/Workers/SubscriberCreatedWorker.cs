
using System.Text.Json;
using Messaging.MessageContracts;
using Messaging.RabbitMQ;

namespace NewsletterService.Workers;

public sealed class SubscriberCreatedWorker : BackgroundService
{
    private readonly IEventConsumer _consumer;
    private readonly ILogger<SubscriberCreatedWorker> _logger;
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    public SubscriberCreatedWorker(IEventConsumer consumer, ILogger<SubscriberCreatedWorker> logger)
    {
        _consumer = consumer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        // Declare queue
        const string queue = "newsletter.subscriber-created";

        while (!ct.IsCancellationRequested)
        {
            try
            {
                // Subscribe to SubscriberCreated fanout exchange
                await _consumer.SubscribeAsync(queue, Exchanges.SubscriberCreated, ct);

                // Start consume loop
                await _consumer.RunAsync(HandleSubscriberCreatedAsync, ct);
            }

            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }

            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SubscriberCreated consumer failed. Retrying in 3 seconds.");

                await Task.Delay(TimeSpan.FromSeconds(3), ct);
            }
        }
    }

    private Task<bool> HandleSubscriberCreatedAsync(ReadOnlyMemory<byte> body, CancellationToken ct)
    {
        SubscriberCreated? payload;
        
        try
        {
            payload = JsonSerializer.Deserialize<SubscriberCreated>(body.Span, JsonOpts);
        }
        catch
        {
            return Task.FromResult(false); // Nack: payload is broken, don't requeue
        }

        if (payload == null) return Task.FromResult(false);

        _logger.LogInformation("Simulated sending welcome email to subscriber {Email}", payload.Email);

        return Task.FromResult(true); // Ack
    }
}