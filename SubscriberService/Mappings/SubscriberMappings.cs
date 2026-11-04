using SubscriberService.Contracts;
using SubscriberService.Entities;

namespace SubscriberService.Mappings;

/// <summary>
/// Maps subscriber contracts and entities.
/// </summary>
public static class SubscriberMappings
{
    public static SubscriberResponse ToResponse(this Subscriber subscriber)
    {
        return new SubscriberResponse
        {
            Id = subscriber.PublicId,
            Email = subscriber.Email,
            SubscriptionTimestampUtc = subscriber.SubscriptionTimestampUtc
        };
    }

    public static Subscriber ToEntity(this CreateSubscriberRequest request)
    {
        return new Subscriber
        {
            Email = request.Email,
        };
    }
}