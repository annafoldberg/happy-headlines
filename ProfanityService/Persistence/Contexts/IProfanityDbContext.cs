using Microsoft.EntityFrameworkCore;
using ProfanityService.Entities;

namespace ProfanityService.Persistence.Contexts;

/// <summary>
/// Defines the profanity database context.
/// </summary>
public interface IProfanityDbContext
{
    DbSet<Profanity> Profanities { get; }
}