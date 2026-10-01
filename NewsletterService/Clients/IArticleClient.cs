using NewsletterService.Contracts;

namespace NewsletterService.Clients;

public interface IArticleClient
{
    Task<ArticleResponse?> GetArticleByDateAsync(ArticleRequest request, CancellationToken ct);
}