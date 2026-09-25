using DraftService.Entities;
using DraftService.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DraftService.Persistence.Repositories;

public class DraftRepository : IDraftRepository
{
    private readonly IDraftDbContext _context;

    public DraftRepository(IDraftDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Draft draft, CancellationToken ct)
    {
        _context.Drafts.Add(draft);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<Draft?> GetByPublicIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Drafts.FirstOrDefaultAsync(d => d.PublicId == id, ct);
    }

    public async Task UpdateAsync(Draft draft, CancellationToken ct)
    {
        _context.Drafts.Update(draft);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Draft draft, CancellationToken ct)
    {
        _context.Drafts.Remove(draft);
        await _context.SaveChangesAsync();
    }
}