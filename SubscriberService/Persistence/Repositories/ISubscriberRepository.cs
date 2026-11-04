using SubscriberService.Entities;

namespace SubscriberService.Persistence.Repositories;

public interface ISubscriberRepository
{
    Task AddAsync(Subscriber subscriber, CancellationToken ct);
    Task<Subscriber?> GetByPublicIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Subscriber>> GetSubscribersAsync(CancellationToken ct);
    Task DeleteAsync(Subscriber subscriber, CancellationToken ct);
}