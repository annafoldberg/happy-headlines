using ArticleService.Contracts;
using ArticleService.Mappings;
using ArticleService.Persistence.Repositories;
using ArticleService.Routing;

namespace ArticleService.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _repository;

    public ArticleService(IArticleRepository repository)
    {
        _repository = repository;
    }

    public async Task<ArticleResponse> CreateArticleAsync(Continent continent, CreateArticleRequest request, CancellationToken ct)
    {
        var article = request.ToEntity();
        
        await _repository.AddAsync(continent, article, ct);

        return article.ToResponse();
    }

    public async Task<ArticleResponse?> GetArticleAsync(Continent continent, Guid id, CancellationToken ct)
    {
        var article = await _repository.GetByPublicIdAsync(continent, id, ct);
        
        if (article is null) return null;
        
        return article.ToResponse();
    }

    public async Task<ArticleOperationResult> UpdateArticleAsync(Continent continent, Guid id, UpdateArticleRequest request, CancellationToken ct)
    {
        var article = await _repository.GetByPublicIdAsync(continent, id, ct);

        if (article is null) return ArticleOperationResult.NotFound;

        var changed = false;

        if (request.Author is not null && request.Author != article.Author)
        {
            article.Author = request.Author;
            changed = true;
        }

        if (request.Title is not null && request.Title != article.Title)
        {
            article.Title = request.Title;
            changed = true;
        }

        if (request.Content is not null && request.Content != article.Content)
        {
            article.Content = request.Content;
            changed = true;
        }

        if (changed)
        {
            article.LastUpdatedTimestampUtc = DateTime.UtcNow;
            await _repository.UpdateAsync(continent, article, ct);
        }

        return ArticleOperationResult.Success;
    }

    public async Task<ArticleOperationResult> DeleteArticleAsync(Continent continent, Guid id, CancellationToken ct)
    {
        var article = await _repository.GetByPublicIdAsync(continent, id, ct);

        if (article is null) return ArticleOperationResult.NotFound;

        await _repository.DeleteAsync(continent, article, ct);

        return ArticleOperationResult.Success;
    }
}