using CommentService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Persistence.Contexts;

public sealed class CommentDbContext : DbContext, ICommentDbContext
{
    public CommentDbContext(DbContextOptions<CommentDbContext> options) : base(options) {}

    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommentDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}