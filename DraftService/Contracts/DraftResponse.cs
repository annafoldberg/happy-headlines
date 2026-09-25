namespace DraftService.Contracts;

public class DraftResponse
{
    public Guid Id { get; set; }
    public string? Author { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime CreationTimestampUtc { get; set; }
    public DateTime? LastUpdatedTimestampUtc { get; set; }
}