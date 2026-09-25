using DraftService.Entities;

namespace DraftService.Persistence.Repositories;

public interface IDraftRepository
{
    Task AddAsync(Draft draft, CancellationToken ct);
    Task<Draft?> GetByPublicIdAsync(Guid id, CancellationToken ct);
    Task UpdateAsync(Draft draft, CancellationToken ct);
    Task DeleteAsync(Draft draft, CancellationToken ct);
}