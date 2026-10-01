using NewsletterService.Clients;
using NewsletterService.Contracts;

namespace NewsletterService.Services;

public class NewsletterService : INewsletterService
{
    private readonly IArticleClient _client;

    public NewsletterService(IArticleClient client)
    {
        _client = client;
    }

    public async Task<NewsletterResponse?> GetDailyNewsletterAsync(Continent continent, DateOnly date, CancellationToken ct)
    {
        var article =  await _client.GetArticleByDateAsync(continent, date, ct);

        if (article is null) return null;

        return new NewsletterResponse
        {
            Date = date,
            Article = article
        };
    }
}