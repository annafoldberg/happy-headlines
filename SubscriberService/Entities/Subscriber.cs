namespace SubscriberService.Entities;

public class Subscriber
{
    public int Id { get; set; }
    public Guid PublicId { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public DateTime SubscriptionTimestampUtc { get; set; } = DateTime.UtcNow;
}