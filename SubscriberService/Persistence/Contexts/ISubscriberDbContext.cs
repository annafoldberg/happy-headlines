using SubscriberService.Entities;
using Microsoft.EntityFrameworkCore;

namespace SubscriberService.Persistence.Contexts;

/// <summary>
/// Defines the subscriber database context.
/// </summary>
public interface ISubscriberDbContext
{
    DbSet<Subscriber> Subscribers { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}