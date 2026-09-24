using CommentService.Clients;
using CommentService.Contracts;
using CommentService.Mappings;
using CommentService.Persistence.Repositories;

namespace CommentService.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;
    private readonly IProfanityClient _client;

    public CommentService(ICommentRepository repository, IProfanityClient client)
    {
        _repository = repository;
        _client = client;
    }

    public async Task<CommentResponse> CreateCommentAsync(Guid articleId, CreateCommentRequest request, CancellationToken ct)
    {
        var filterRequest = request.ToFilterRequest();
        var filterResponse = await _client.FilterCommentAsync(filterRequest, ct);

        var comment = request.ToEntity(articleId, filterResponse.Comment);

        await _repository.AddAsync(comment, ct);

        return comment.ToResponse();
    }

    public async Task<CommentResponse?> GetCommentAsync(Guid id, CancellationToken ct)
    {
        var comment = await _repository.GetByPublicIdAsync(id, ct);

        if (comment is null) return null;

        return comment.ToResponse();
    }

    public async Task<CommentOperationResult> UpdateCommentAsync(Guid id, UpdateCommentRequest request, CancellationToken ct)
    {
        var comment = await _repository.GetByPublicIdAsync(id, ct);

        if (comment is null) return CommentOperationResult.NotFound;
        
        var changed = false;

        if (request.Author is not null && request.Author != comment.Author)
        {
            comment.Author = request.Author;
            changed = true;
        }

        if (request.Content is not null && request.Content != comment.Content)
        {
            comment.Content = request.Content;
            changed = true;
        }

        if (changed)
        {
            comment.LastUpdatedTimestampUtc = DateTime.UtcNow;
            await _repository.UpdateAsync(comment, ct);
        }

        return CommentOperationResult.Success;
    }

    public async Task<CommentOperationResult> DeleteCommentAsync(Guid id, CancellationToken ct)
    {
        var comment = await _repository.GetByPublicIdAsync(id, ct);

        if (comment is null) return CommentOperationResult.NotFound;

        await _repository.DeleteAsync(comment, ct);

        return CommentOperationResult.Success;
    }
}