using NewsletterService.Contracts;

namespace NewsletterService.Clients;

public interface ISubscriberClient
{
    Task<IReadOnlyList<SubscriberResponse>> GetSubscribersAsync(CancellationToken ct);
}