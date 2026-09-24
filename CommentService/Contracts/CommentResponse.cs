namespace CommentService.Contracts;

public class CommentResponse
{
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreationTimestampUtc { get; set; }
    public DateTime? LastUpdatedTimestampUtc { get; set; }
}