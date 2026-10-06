using ArticleService.Entities;

namespace ArticleService.Caching;

public interface IArticleCache
{
    Task<Article?> GetByPublicIdAsync(Guid id, CancellationToken ct);
    Task SetAsync(Article article, CancellationToken ct);
}