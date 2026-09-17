using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleService.Entities;

/// <summary>
/// Represents a published article.
/// </summary>
[Table("articles")]
public class Article
{
    [Column("id")]
    public int Id { get; set; }

    [Column("public_id")]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    [Column("publication_timestamp_utc")]
    public DateTime PublicationTimestampUtc { get; set; } = DateTime.UtcNow;

    [Column("last_updated_timestamp_utc")]
    public DateTime? LastUpdatedTimestampUtc { get; set; }

    [Column("author")]
    public string Author { get; set; } = string.Empty;

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("content")]
    public string Content { get; set; } = string.Empty;
}