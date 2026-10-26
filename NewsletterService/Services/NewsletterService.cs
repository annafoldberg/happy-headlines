using System.Diagnostics;
using NewsletterService.Clients;
using NewsletterService.Contracts;

namespace NewsletterService.Services;

public class NewsletterService : INewsletterService
{
    private readonly IArticleClient _client;
    private static readonly ActivitySource _activitySource = new("NewsletterService");

    public NewsletterService(IArticleClient client)
    {
        _client = client;
    }

    public async Task<NewsletterResponse?> GetDailyNewsletterAsync(Continent continent, DateOnly date, CancellationToken ct)
    {
        using var activity = _activitySource.StartActivity("GetDailyNewsletter");

        var article =  await _client.GetArticleByDateAsync(continent, date, ct);

        if (article is null) return null;

        return new NewsletterResponse
        {
            Date = date,
            Article = article
        };
    }
}