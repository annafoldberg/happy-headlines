namespace NewsletterService.Contracts;

public class ArticleRequest
{
    public DateOnly PublicationDate { get; set; }
    public Continent Continent { get; set; }
}