using DraftService.Contracts;

namespace DraftService.Services;

public interface IDraftService
{
    Task<DraftResponse> CreateDraftAsync(DraftRequest request, CancellationToken ct);
    Task<DraftResponse?> GetDraftAsync(Guid id, CancellationToken ct);
    Task<DraftOperationResult> UpdateDraftAsync(Guid id, DraftRequest request, CancellationToken ct);
    Task<DraftOperationResult> DeleteDraftAsync(Guid id, CancellationToken ct);
}