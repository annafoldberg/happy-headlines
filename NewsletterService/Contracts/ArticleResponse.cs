namespace NewsletterService.Contracts;

public class ArticleResponse
{
    public Guid Id { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublicationTimestampUtc { get; set; }
}