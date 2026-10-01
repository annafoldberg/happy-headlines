using PublisherService.Contracts;
using PublisherService.Mappings;
using Messaging.RabbitMQ;

namespace PublisherService.Services;

public class PublisherService : IPublisherService
{
    private readonly IEventPublisher _publisher;

    public PublisherService(IEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task PublishArticleAsync(CreatePublicationRequest request, CancellationToken ct)
    {
        var publication = request.ToArticlePublished();

        await _publisher.PublishAsync(Exchanges.ArticlePublished, "", publication, ct: ct);
    }
}