
using System.Text.Json;
using Messaging.MessageContracts;
using Messaging.RabbitMQ;

namespace NewsletterService.Workers;

public sealed class ArticlePublishedWorker : BackgroundService
{
    private readonly IEventConsumer _consumer;
    private readonly ILogger<ArticlePublishedWorker> _logger;
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    public ArticlePublishedWorker(IEventConsumer consumer, ILogger<ArticlePublishedWorker> logger)
    {
        _consumer = consumer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        // Declare queue
        const string queue = "newsletter.article-published";

        while (!ct.IsCancellationRequested)
        {
            try
            {
                // Subscribe to ArticlePublished fanout exchange
                await _consumer.SubscribeAsync(queue, Exchanges.ArticlePublished, ct);

                // Start consume loop
                await _consumer.RunAsync(HandleArticlePublishedAsync, ct);
            }

            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }

            catch (Exception ex)
            {
                _logger.LogWarning(ex, "ArticlePublished consumer failed. Retrying in 3 seconds.");
                
                await Task.Delay(TimeSpan.FromSeconds(3), ct);
            }
        }
    }

    private Task<bool> HandleArticlePublishedAsync(ReadOnlyMemory<byte> body, CancellationToken ct)
    {
        ArticlePublished? payload;
        
        try
        {
            payload = JsonSerializer.Deserialize<ArticlePublished>(body.Span, JsonOpts);
        }
        catch
        {
            return Task.FromResult(false); // Nack: payload is broken, don't requeue
        }

        if (payload == null) return Task.FromResult(false);

        _logger.LogInformation("Received article {Title}", payload.Title);

        return Task.FromResult(true); // Ack
    }
}