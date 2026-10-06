using CommentService.Entities;

namespace CommentService.Persistence.Repositories;

public interface ICommentRepository
{
    Task AddAsync(Comment comment, CancellationToken ct);
    Task<IReadOnlyList<Comment>> GetByArticleIdAsync(Guid articleId, CancellationToken ct);
    Task<Comment?> GetByPublicIdAsync(Guid id, CancellationToken ct);
    Task UpdateAsync(Comment comment, CancellationToken ct);
    Task DeleteAsync(Comment comment, CancellationToken ct);
}