using ArticleService.Caching;
using ArticleService.Persistence.Repositories;
using ArticleService.Routing;

namespace ArticleService.Workers;

public sealed class ArticleCacheRefreshWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ArticleCacheRefreshWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var repository = scope.ServiceProvider.GetRequiredService<IArticleRepository>();

            var cache = scope.ServiceProvider.GetRequiredService<IArticleCache>();

            var fromUtc = DateTime.UtcNow.AddDays(-14);

            var articles = await repository.GetRecentAsync(Continent.Global, fromUtc, ct);

            foreach (var article in articles) await cache.SetAsync(article, ct);

            await Task.Delay(TimeSpan.FromMinutes(5), ct);
        }
    }
}