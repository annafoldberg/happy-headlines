using ProfanityService.Contracts;

namespace ProfanityService.Services;

public interface IProfanityFilterService
{
    Task<FilterContentResponse> FilterContentAsync(FilterContentRequest request, CancellationToken ct);
}