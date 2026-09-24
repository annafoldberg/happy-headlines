using CommentService.Contracts;

namespace CommentService.Clients;

public class ProfanityClient : IProfanityClient
{
    private readonly HttpClient _httpClient;

    public ProfanityClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<FilterCommentResponse> FilterCommentAsync(FilterCommentRequest request, CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync("profanities/filter", request, ct);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<FilterCommentResponse>(cancellationToken: ct);

        if (result is null)
            throw new InvalidOperationException("ProfanityService returned an empty response.");

        return result;
    }
}