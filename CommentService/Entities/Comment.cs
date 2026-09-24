namespace CommentService.Entities;

public class Comment
{
    public int Id { get; set; }
    public Guid PublicId { get; set; } = Guid.NewGuid();
    public Guid ArticleId { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreationTimestampUtc { get; set; } = DateTime.UtcNow;
    public DateTime? LastUpdatedTimestampUtc { get; set; }
}