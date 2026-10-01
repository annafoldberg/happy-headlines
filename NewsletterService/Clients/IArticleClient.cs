using NewsletterService.Contracts;

namespace NewsletterService.Clients;

public interface IArticleClient
{
    Task<ArticleResponse?> GetArticleByDateAsync(Continent continent, DateOnly date, CancellationToken ct);
}