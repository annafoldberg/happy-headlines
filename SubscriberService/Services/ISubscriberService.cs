using SubscriberService.Contracts;

namespace SubscriberService.Services;

public interface ISubscriberService
{
    Task<(CreateSubscriberOperationResult Result, SubscriberResponse? Subscriber)> CreateSubscriberAsync(CreateSubscriberRequest request, CancellationToken ct);
    Task<IReadOnlyList<SubscriberResponse>> GetSubscribersAsync(CancellationToken ct);
    Task<DeleteSubscriberOperationResult> DeleteSubscriberAsync(Guid id, CancellationToken ct);
}