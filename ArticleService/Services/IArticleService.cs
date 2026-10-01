using ArticleService.Contracts;
using ArticleService.Routing;
using ArticlePublished = Messaging.MessageContracts.ArticlePublished;

namespace ArticleService.Services;

public interface IArticleService
{
    Task CreateFromPublishedArticleAsync(ArticlePublished articlePublished, CancellationToken ct);
    Task<ArticleResponse> CreateArticleAsync(Continent continent, CreateArticleRequest request, CancellationToken ct);
    Task<ArticleResponse?> GetArticleByIdAsync(Continent continent, Guid id, CancellationToken ct);
    Task<ArticleResponse?> GetArticleByDateAsync(Continent continent, DateOnly publicationDate, CancellationToken ct);
    Task<ArticleOperationResult> UpdateArticleAsync(Continent continent, Guid id, UpdateArticleRequest request, CancellationToken ct);
    Task<ArticleOperationResult> DeleteArticleAsync(Continent continent, Guid id, CancellationToken ct);
}