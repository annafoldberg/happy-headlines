using ProfanityService.Contracts;

namespace ProfanityService.Services;

public interface IProfanityFilterService
{
    Task<FilterCommentResponse> FilterCommentAsync(FilterCommentRequest request, CancellationToken ct);
}