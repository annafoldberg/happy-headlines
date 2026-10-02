using DraftService.Contracts;
using DraftService.Mappings;
using DraftService.Persistence.Repositories;
using Monitoring;

namespace DraftService.Services;

public class DraftService : IDraftService
{
    private readonly IDraftRepository _repository;
    private readonly ILogger<DraftService> _logger;

    public DraftService(IDraftRepository repository, ILogger<DraftService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<DraftResponse> CreateDraftAsync(DraftRequest request, CancellationToken ct)
    {
        using var activity = TracingSource.ActivitySource.StartActivity("CreateDraft");

        var draft = request.ToEntity();

        await _repository.AddAsync(draft, ct);

        _logger.LogInformation("Draft {DraftId} created.", draft.PublicId);

        return draft.ToResponse();
    }

    public async Task<DraftResponse?> GetDraftAsync(Guid id, CancellationToken ct)
    {
        using var activity = TracingSource.ActivitySource.StartActivity("GetDraft");
        
        var draft = await _repository.GetByPublicIdAsync(id, ct);
        
        if (draft is null)
        {
            _logger.LogDebug("Draft {DraftId} was not found.", id);
            return null;
        }

        return draft.ToResponse();
    }

    public async Task<DraftOperationResult> UpdateDraftAsync(Guid id, DraftRequest request, CancellationToken ct)
    {
        using var activity = TracingSource.ActivitySource.StartActivity("UpdateDraft");

        var draft = await _repository.GetByPublicIdAsync(id, ct);

        if (draft is null)
        {
            _logger.LogWarning("Draft could not be updated because draft {DraftId} was not found.", id);
            return DraftOperationResult.NotFound;
        }

        draft.Author = request.Author;
        draft.Title = request.Title;
        draft.Content = request.Content;

        draft.LastUpdatedTimestampUtc = DateTime.UtcNow;
        await _repository.UpdateAsync(draft, ct);

        _logger.LogInformation("Draft {DraftId} was updated.", id);

        return DraftOperationResult.Success;
    }

    public async Task<DraftOperationResult> DeleteDraftAsync(Guid id, CancellationToken ct)
    {
        using var activity = TracingSource.ActivitySource.StartActivity("DeleteDraft");

        var draft = await _repository.GetByPublicIdAsync(id, ct);

        if (draft is null)
        {
            _logger.LogWarning("Draft could not be deleted because draft {DraftId} was not found.", id);
            return DraftOperationResult.NotFound;
        }

        await _repository.DeleteAsync(draft, ct);
        _logger.LogInformation("Draft {DraftId} was deleted.", id);

        return DraftOperationResult.Success;
    }
}