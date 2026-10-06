using CommentService.Caching;
using CommentService.Clients;
using CommentService.Contracts;
using CommentService.Mappings;
using CommentService.Persistence.Repositories;

namespace CommentService.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;
    private readonly ICommentCache _cache;
    private readonly IProfanityClient _client;

    public CommentService(ICommentRepository repository, ICommentCache cache, IProfanityClient client)
    {
        _repository = repository;
        _cache = cache;
        _client = client;
    }

    public async Task<CommentResponse> CreateCommentAsync(Guid articleId, CreateCommentRequest request, CancellationToken ct)
    {
        var filterRequest = new FilterCommentRequest { Comment = request.Content };
        var filterResponse = await _client.FilterCommentAsync(filterRequest, ct);

        var comment = request.ToEntity(articleId, filterResponse.Comment);

        await _repository.AddAsync(comment, ct);

        // Remove cached comments so the new comment is included on the next read
        await _cache.RemoveAsync(articleId, ct);

        return comment.ToResponse();
    }

    public async Task<IReadOnlyList<CommentResponse>> GetCommentsByArticleIdAsync(Guid articleId, CancellationToken ct)
    {
        var comments = await _cache.GetByArticleIdAsync(articleId, ct);

        if (comments is null)
        {
            comments = await _repository.GetByArticleIdAsync(articleId, ct);

            // Add comments to cache
            await _cache.SetAsync(articleId, comments, ct);
        }

        return comments.Select(comment => comment.ToResponse()).ToList();
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
            var filterRequest = new FilterCommentRequest { Comment = request.Content };
            var filterResponse = await _client.FilterCommentAsync(filterRequest, ct);

            comment.Content = filterResponse.Comment;
            changed = true;
        }

        if (changed)
        {
            comment.LastUpdatedTimestampUtc = DateTime.UtcNow;
            await _repository.UpdateAsync(comment, ct);
                
            // Remove cached comments so the updated comment is reflected on the next read
            await _cache.RemoveAsync(comment.ArticleId, ct);
        }

        return CommentOperationResult.Success;
    }

    public async Task<CommentOperationResult> DeleteCommentAsync(Guid id, CancellationToken ct)
    {
        var comment = await _repository.GetByPublicIdAsync(id, ct);

        if (comment is null) return CommentOperationResult.NotFound;

        await _repository.DeleteAsync(comment, ct);

        // Remove cached comments so the deleted comment is omitted on the next read
        await _cache.RemoveAsync(comment.ArticleId, ct);

        return CommentOperationResult.Success;
    }
}