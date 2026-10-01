using NewsletterService.Contracts;

namespace NewsletterService.Services;

public interface INewsletterService
{
    Task<NewsletterResponse?> GetDailyNewsletterAsync(Continent continent, DateOnly date, CancellationToken ct);
}