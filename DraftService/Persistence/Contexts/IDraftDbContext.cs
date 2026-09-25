using DraftService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DraftService.Persistence.Contexts;

/// <summary>
/// Defines the draft database context.
/// </summary>
public interface IDraftDbContext
{
    DbSet<Draft> Drafts { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}