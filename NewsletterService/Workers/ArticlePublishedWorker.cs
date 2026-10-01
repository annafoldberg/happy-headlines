
using System.Text.Json;
using Messaging.MessageContracts;
using Messaging.RabbitMQ;

namespace NewsletterService.Workers;

public sealed class ArticlePublishedWorker : BackgroundService
{
    private readonly IEventConsumer _consumer;
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    public ArticlePublishedWorker(IEventConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        try
        {
            // Declare queue
            const string queue = "newsletter.article-published";

            // Subscribe to ArticlePublished fanout exchange
            await _consumer.SubscribeAsync(queue, Exchanges.ArticlePublished, ct);

            // Start consume loop
            await _consumer.RunAsync(HandleArticlePublishedAsync, ct);
        }
        catch (Exception ex)
        {
            await Task.Delay(TimeSpan.FromSeconds(3), ct);
        }
    }

    private Task<bool> HandleArticlePublishedAsync(ReadOnlyMemory<byte> body, CancellationToken ct)
    {
        ArticlePublished? payload;
        
        try
        {
            payload = JsonSerializer.Deserialize<ArticlePublished>(body.Span, JsonOpts);
        }
        catch (Exception ex)
        {
            return Task.FromResult(false); // Nack: payload is broken, don't requeue
        }

        if (payload == null) return Task.FromResult(false);

        Console.WriteLine($"Received article {payload.Title}");

        return Task.FromResult(false); // Ack
    }
}