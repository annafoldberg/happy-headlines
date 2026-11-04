namespace SubscriberService.Contracts;

public class SubscriberResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime SubscriptionTimestampUtc { get; set; }
}