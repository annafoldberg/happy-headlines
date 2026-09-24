using ArticleService.Routing;

namespace ArticleService.Persistence.Contexts;

/// <summary>
/// Defines a factory for creating article database contexts.
/// </summary>
public interface IArticleDbContextFactory
{
    ArticleDbContext Create(Continent continent);
}