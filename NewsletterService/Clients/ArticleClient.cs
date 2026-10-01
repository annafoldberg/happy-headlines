using NewsletterService.Contracts;

namespace NewsletterService.Clients;

public class ArticleClient : IArticleClient
{
    private readonly HttpClient _httpClient;

    public ArticleClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ArticleResponse?> GetArticleByDateAsync(Continent continent, DateOnly date, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync($"articles/{continent}?publicationDate={date:yyyy-MM-dd}", ct);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ArticleResponse>(cancellationToken: ct);

        if (result is null)
            throw new InvalidOperationException("ArticleService returned an empty response.");

        return result;
    }
}