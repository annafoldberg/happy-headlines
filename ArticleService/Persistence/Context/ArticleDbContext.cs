using ArticleService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArticleService.Persistence.Context;

/// <summary>
/// Entity Framework Core database context for the article database.
/// </summary>
public sealed class ArticleDbContext : DbContext, IArticleDbContext
{
    public ArticleDbContext(DbContextOptions<ArticleDbContext> options) : base(options) {}

    public DbSet<Article> Articles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArticleDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}