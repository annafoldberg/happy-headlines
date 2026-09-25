using DraftService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DraftService.Persistence.Contexts;

public sealed class DraftDbContext : DbContext, IDraftDbContext
{
    public DraftDbContext(DbContextOptions<DraftDbContext> options) : base(options) {}

    public DbSet<Draft> Drafts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DraftDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}