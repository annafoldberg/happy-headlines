namespace NewsletterService.Contracts;

public class NewsletterResponse
{
    public DateOnly Date { get; set; }
    public ArticleResponse Article { get; set; } = null!;
}