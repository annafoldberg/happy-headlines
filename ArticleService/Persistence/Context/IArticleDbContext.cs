using ArticleService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArticleService.Persistence.Context;

/// <summary>
/// Defines the article database context.
/// </summary>
public interface IArticleDbContext
{
    DbSet<Article> Articles { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}