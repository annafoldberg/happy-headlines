using System.Diagnostics;
using NewsletterService.Clients;
using NewsletterService.Contracts;

namespace NewsletterService.Services;

public class NewsletterService : INewsletterService
{
    private readonly IArticleClient _articleClient;
    private readonly ISubscriberClient _subscriberClient;
    private readonly ILogger<NewsletterService> _logger;
    private static readonly ActivitySource _activitySource = new("NewsletterService");

    public NewsletterService(
        IArticleClient articleClient,
        ISubscriberClient subscriberClient,
        ILogger<NewsletterService> logger)
    {
        _articleClient = articleClient;
        _subscriberClient = subscriberClient;
        _logger = logger;
    }

    public async Task<NewsletterResponse?> GetDailyNewsletterAsync(Continent continent, DateOnly date, CancellationToken ct)
    {
        using var activity = _activitySource.StartActivity("GetDailyNewsletter");

        var article =  await _articleClient.GetArticleByDateAsync(continent, date, ct);

        if (article is null) return null;

        return new NewsletterResponse
        {
            Date = date,
            Article = article
        };
    }

    public async Task<IReadOnlyList<SubscriberResponse>> GetSubscribersAsync(CancellationToken ct)
    {
        try
        {
            return await _subscriberClient.GetSubscribersAsync(ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "SubscriberService is unavailable.");
            throw;
        }
    }
}