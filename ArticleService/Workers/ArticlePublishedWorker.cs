
using System.Text.Json;
using ArticleService.Services;
using Messaging.MessageContracts;
using Messaging.RabbitMQ;

namespace ArticleService.Workers;

public sealed class ArticlePublishedWorker : BackgroundService
{
    private readonly IEventConsumer _consumer;
    private readonly IServiceScopeFactory _scopeFactory;
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    public ArticlePublishedWorker(IEventConsumer consumer, IServiceScopeFactory scopeFactory)
    {
        _consumer = consumer;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        try
        {
            // Declare queue
            const string queue = "article.article-published";

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

    private async Task<bool> HandleArticlePublishedAsync(ReadOnlyMemory<byte> body, CancellationToken ct)
    {
        ArticlePublished? payload;
        
        try
        {
            payload = JsonSerializer.Deserialize<ArticlePublished>(body.Span, JsonOpts);
        }
        catch (Exception ex)
        {
            return false; // Nack: payload is broken, don't requeue
        }

        if (payload == null) return false;

        using var scope = _scopeFactory.CreateScope();

        var articleService = scope.ServiceProvider.GetRequiredService<IArticleService>();
        
        await articleService.CreateFromPublishedArticleAsync(payload, ct);

        return true; // Ack
    }
}