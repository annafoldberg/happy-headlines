namespace ArticleService.Dtos;

/// <summary>
/// Represents a published article DTO.
/// </summary>
public class ArticleDto
{
    public Guid Id { get; set; }
    public DateTime PublicationTimestampUtc { get; set; }
    public DateTime? LastUpdatedTimestampUtc { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}