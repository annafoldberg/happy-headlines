using CommentService.Entities;

namespace CommentService.Caching;

public interface ICommentCache
{
    Task<IReadOnlyList<Comment>?> GetByArticleIdAsync(Guid articleId, CancellationToken ct);
    Task SetAsync(Guid articleId, IReadOnlyList<Comment> comments, CancellationToken ct);
    Task RemoveAsync(Guid articleId, CancellationToken ct);
}