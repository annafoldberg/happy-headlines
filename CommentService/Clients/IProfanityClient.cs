using CommentService.Contracts;

namespace CommentService.Clients;

public interface IProfanityClient
{
    Task<FilterContentResponse> FilterContentAsync(FilterContentRequest request, CancellationToken ct);
}