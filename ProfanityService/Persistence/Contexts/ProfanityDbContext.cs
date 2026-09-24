using ProfanityService.Entities;
using Microsoft.EntityFrameworkCore;
using ProfanityService.Persistence.Contexts;

namespace ProfanityService.Persistence.Contexts;

/// <summary>
/// Entity Framework Core database context for the profanity database.
/// </summary>
public sealed class ProfanityDbContext : DbContext, IProfanityDbContext
{
    public ProfanityDbContext(DbContextOptions<ProfanityDbContext> options) : base(options) {}

    public DbSet<Profanity> Profanities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfanityDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}