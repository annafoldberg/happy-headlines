using ArticleService.Routing;

namespace ArticleService.Persistence.Context;

/// <summary>
/// Defines a factory for creating article database contexts.
/// </summary>
public interface IArticleDbContextFactory
{
    ArticleDbContext Create(Continent continent);
}