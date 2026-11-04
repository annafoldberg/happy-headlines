namespace Messaging.MessageContracts;

/// <summary>
/// Event published when a subscriber is created.
/// Produced by Subscriber Service, consumed by Newsletter Service.
/// </summary>
public sealed record SubscriberCreated(string Email);