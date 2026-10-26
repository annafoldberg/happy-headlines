using PublisherService.Contracts;
using PublisherService.Mappings;
using Messaging.RabbitMQ;
using System.Diagnostics;

namespace PublisherService.Services;

public class PublisherService : IPublisherService
{
    private readonly IEventPublisher _publisher;
    private static readonly ActivitySource _activitySource = new("PublisherService");

    public PublisherService(IEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task PublishArticleAsync(CreatePublicationRequest request, CancellationToken ct)
    {
        using var activity = _activitySource.StartActivity("PublishArticle");

        var publication = request.ToArticlePublished();

        await _publisher.PublishAsync(Exchanges.ArticlePublished, "", publication, ct: ct);
    }
}