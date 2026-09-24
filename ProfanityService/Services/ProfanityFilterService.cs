using System.Text.RegularExpressions;
using ProfanityService.Contracts;
using ProfanityService.Persistence.Repositories;

namespace ProfanityService.Services;

public class ProfanityFilterService : IProfanityFilterService
{
    private readonly IProfanityRepository _repository;

    public ProfanityFilterService(IProfanityRepository repository)
    {
        _repository = repository;
    }

    public async Task<FilterCommentResponse> FilterCommentAsync(FilterCommentRequest request, CancellationToken ct)
    {
        var profanities = await _repository.GetAllTermsAsync(ct);

        var profanitySet = profanities.ToHashSet(StringComparer.OrdinalIgnoreCase);
        
        var filteredComment = Regex.Replace(
            request.Comment,
            @"\b[\p{L}']+\b",
            match => profanitySet.Contains(match.Value)
                ? new string('*', match.Value.Length)
                : match.Value);

        return new FilterCommentResponse { Comment = filteredComment };
    }
}