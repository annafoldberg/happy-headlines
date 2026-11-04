using SubscriberService.Entities;
using SubscriberService.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SubscriberService.Exceptions;

namespace SubscriberService.Persistence.Repositories;

public class SubscriberRepository : ISubscriberRepository
{
    private readonly ISubscriberDbContext _context;

    public SubscriberRepository(ISubscriberDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Subscriber subscriber, CancellationToken ct)
    {
        _context.Subscribers.Add(subscriber);
        
        // Handle unique violation
        try
        {
            await _context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation,
                ConstraintName: "ux_subscribers_email_lower"
            })
        {
            throw new DuplicateSubscriberException();
        }
    }

    public async Task<Subscriber?> GetByPublicIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Subscribers
            .FirstOrDefaultAsync(subscriber => subscriber.PublicId == id, ct);
    }

    public async Task<IReadOnlyList<Subscriber>> GetSubscribersAsync(CancellationToken ct)
    {
        return await _context.Subscribers.ToListAsync(ct);
    }

    public async Task DeleteAsync(Subscriber subscriber, CancellationToken ct)
    {
        _context.Subscribers.Remove(subscriber);
        await _context.SaveChangesAsync(ct);
    }
}