using Messaging.MessageContracts;
using Messaging.RabbitMQ;
using SubscriberService.Contracts;
using SubscriberService.Exceptions;
using SubscriberService.Mappings;
using SubscriberService.Persistence.Repositories;

namespace SubscriberService.Services;

public class SubscriberService : ISubscriberService
{
    private readonly IEventPublisher _publisher;
    private readonly ISubscriberRepository _repository;

    public SubscriberService(ISubscriberRepository repository, IEventPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<(CreateSubscriberOperationResult Result, SubscriberResponse? Subscriber)>
        CreateSubscriberAsync(CreateSubscriberRequest request, CancellationToken ct)
    {
        var subscriber = request.ToEntity();

        // Handle subscriber already exists
        try
        {
            await _repository.AddAsync(subscriber, ct);

            await _publisher.PublishAsync(
                Exchanges.SubscriberCreated,
                "",
                new SubscriberCreated(subscriber.Email),
                ct: ct);

            return (CreateSubscriberOperationResult.Success, subscriber.ToResponse());
        }
        catch (DuplicateSubscriberException)
        {
            return (CreateSubscriberOperationResult.AlreadyExists, null);
        }
    }

    public async Task<IReadOnlyList<SubscriberResponse>> GetSubscribersAsync(CancellationToken ct)
    {
        var subscribers = await _repository.GetSubscribersAsync(ct);

        return subscribers.Select(subscriber => subscriber.ToResponse()).ToList();
    }

    public async Task<DeleteSubscriberOperationResult> DeleteSubscriberAsync(Guid id, CancellationToken ct)
    {
        var subscriber = await _repository.GetByPublicIdAsync(id, ct);

        if (subscriber is null) return DeleteSubscriberOperationResult.NotFound;

        await _repository.DeleteAsync(subscriber, ct);

        return DeleteSubscriberOperationResult.Success;
    }
}