using Microsoft.EntityFrameworkCore;
using ProfanityService.Persistence.Contexts;

namespace ProfanityService.Persistence.Repositories;

public class ProfanityRepository : IProfanityRepository
{
    private readonly IProfanityDbContext _context;

    public ProfanityRepository(IProfanityDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<string>> GetAllTermsAsync(CancellationToken ct)
    {
        return await _context.Profanities
            .AsNoTracking()
            .Select(p => p.Term)
            .ToListAsync(ct);
    }
}