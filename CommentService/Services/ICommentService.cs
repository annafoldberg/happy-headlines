using CommentService.Contracts;

namespace CommentService.Services;

public interface ICommentService
{
    Task<CommentResponse> CreateCommentAsync(Guid articleId, CreateCommentRequest request, CancellationToken ct);
    Task<CommentResponse?> GetCommentAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<CommentResponse>> GetCommentsByArticleIdAsync(Guid articleId, CancellationToken ct);
    Task<CommentOperationResult> UpdateCommentAsync(Guid id, UpdateCommentRequest request, CancellationToken ct);
    Task<CommentOperationResult> DeleteCommentAsync(Guid id, CancellationToken ct);
}