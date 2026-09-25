using DraftService.Contracts;
using DraftService.Mappings;
using DraftService.Persistence.Repositories;

namespace DraftService.Services;

public class DraftService : IDraftService
{
    private readonly IDraftRepository _repository;

    public DraftService(IDraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<DraftResponse> CreateDraftAsync(DraftRequest request, CancellationToken ct)
    {
        var draft = request.ToEntity();

        await _repository.AddAsync(draft, ct);

        return draft.ToResponse();
    }

    public async Task<DraftResponse?> GetDraftAsync(Guid id, CancellationToken ct)
    {
        var draft = await _repository.GetByPublicIdAsync(id, ct);
        
        if (draft is null) return null;

        return draft.ToResponse();
    }

    public async Task<DraftOperationResult> UpdateDraftAsync(Guid id, DraftRequest request, CancellationToken ct)
    {
        var draft = await _repository.GetByPublicIdAsync(id, ct);

        if (draft is null) return DraftOperationResult.NotFound;

        draft.Author = request.Author;
        draft.Title = request.Title;
        draft.Content = request.Content;

        draft.LastUpdatedTimestampUtc = DateTime.UtcNow;
        await _repository.UpdateAsync(draft, ct);

        return DraftOperationResult.Success;
    }

    public async Task<DraftOperationResult> DeleteDraftAsync(Guid id, CancellationToken ct)
    {
        var draft = await _repository.GetByPublicIdAsync(id, ct);

        if (draft is null) return DraftOperationResult.NotFound;

        await _repository.DeleteAsync(draft, ct);

        return DraftOperationResult.Success;
    }
}