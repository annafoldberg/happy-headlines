using PublisherService.Contracts;

namespace PublisherService.Services;

public interface IPublisherService
{
    Task PublishArticleAsync(CreatePublicationRequest request, CancellationToken ct);
}