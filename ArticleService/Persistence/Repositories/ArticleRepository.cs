using ArticleService.Entities;
using ArticleService.Persistence.Contexts;
using ArticleService.Routing;
using Microsoft.EntityFrameworkCore;

namespace ArticleService.Persistence.Repositories;

/// <summary>
/// Provides CRUD operations for published articles across continent-specific databases.
/// </summary>
public class ArticleRepository : IArticleRepository
{
    private readonly IArticleDbContextFactory _contextFactory;

    public ArticleRepository(IArticleDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task AddAsync(Continent continent, Article article, CancellationToken ct)
    {
        await using var context = _contextFactory.Create(continent);

        context.Articles.Add(article);
        await context.SaveChangesAsync(ct);
    }

    public async Task<Article?> GetByPublicIdAsync(Continent continent, Guid id, CancellationToken ct)
    {
        await using var context = _contextFactory.Create(continent);

        return await context.Articles.FirstOrDefaultAsync(a => a.PublicId == id, ct);
    }

    public async Task<IReadOnlyList<Article>> GetByDateAsync(Continent continent, DateOnly publicationDate, CancellationToken ct)
    {
        await using var context = _contextFactory.Create(continent);

        var start = publicationDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var end = start.AddDays(1);

        return await context.Articles
            .Where(a =>
                a.PublicationTimestampUtc >= start && 
                a.PublicationTimestampUtc < end)
            .ToListAsync(ct);
    }

    public async Task UpdateAsync(Continent continent, Article article, CancellationToken ct)
    {
        await using var context = _contextFactory.Create(continent);

        context.Articles.Update(article);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Continent continent, Article article, CancellationToken ct)
    {
        await using var context = _contextFactory.Create(continent);

        context.Articles.Remove(article);
        await context.SaveChangesAsync(ct);
    }
}