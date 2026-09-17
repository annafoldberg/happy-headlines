using ArticleService.Entities;
using ArticleService.Routing;

namespace ArticleService.Persistence.Repositories;

public interface IArticleRepository
{
    Task AddAsync(Continent continent, Article article, CancellationToken ct);
    Task<Article?> GetByIdAsync(Continent continent, Guid id, CancellationToken ct);
    Task UpdateAsync(Continent continent, Article article, CancellationToken ct);
    Task DeleteAsync(Continent continent, Article article, CancellationToken ct);
}