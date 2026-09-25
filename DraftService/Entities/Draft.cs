namespace DraftService.Entities;

public class Draft
{
    public int Id { get; set; }
    public Guid PublicId { get; set; } = Guid.NewGuid();
    public string? Author { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime CreationTimestampUtc { get; set; } = DateTime.UtcNow;
    public DateTime? LastUpdatedTimestampUtc { get; set; }
}