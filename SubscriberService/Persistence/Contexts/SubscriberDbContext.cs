using SubscriberService.Entities;
using Microsoft.EntityFrameworkCore;

namespace SubscriberService.Persistence.Contexts;

public sealed class SubscriberDbContext : DbContext, ISubscriberDbContext
{
    public SubscriberDbContext(DbContextOptions<SubscriberDbContext> options) : base(options) {}

    public DbSet<Subscriber> Subscribers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SubscriberDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}