using CommentService.Contracts;

namespace CommentService.Clients;

public interface IProfanityClient
{
    Task<FilterCommentResponse> FilterCommentAsync(FilterCommentRequest request, CancellationToken ct);
}