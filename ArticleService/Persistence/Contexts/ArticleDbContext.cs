using ArticleService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArticleService.Persistence.Contexts;

/// <summary>
/// Entity Framework Core database context for the Article Service.
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