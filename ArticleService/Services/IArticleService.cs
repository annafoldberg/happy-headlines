using ArticleService.Contracts;
using ArticleService.Routing;

namespace ArticleService.Services;

public interface IArticleService
{
    Task<ArticleResponse> CreateArticleAsync(Continent continent, CreateArticleRequest request, CancellationToken ct);
    Task<ArticleResponse?> GetArticleAsync(Continent continent, Guid id, CancellationToken ct);
    Task<ArticleOperationResult> UpdateArticleAsync(Continent continent, Guid id, UpdateArticleRequest request, CancellationToken ct);
    Task<ArticleOperationResult> DeleteArticleAsync(Continent continent, Guid id, CancellationToken ct);
}