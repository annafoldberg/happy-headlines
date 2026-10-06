using System.Text.Json;
using ArticleService.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics.Metrics;

namespace ArticleService.Caching;

public class ArticleCache : IArticleCache
{
    private readonly IDistributedCache _cache;
    private static readonly Meter Meter = new("ArticleService.Caching");
    private static readonly Counter<long> CacheHits = Meter.CreateCounter<long>("article_cache_hits");
    private static readonly Counter<long> CacheMisses = Meter.CreateCounter<long>("article_cache_misses");

    public ArticleCache(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<Article?> GetByPublicIdAsync(Guid id, CancellationToken ct)
    {
        var value = await _cache.GetStringAsync($"article:{id}", ct);

        if (value is null)
        {
            CacheMisses.Add(1);
            return null;
        }

        CacheHits.Add(1);

        return JsonSerializer.Deserialize<Article>(value);
    }

    public async Task SetAsync(Article article, CancellationToken ct)
    {
        var value = JsonSerializer.Serialize(article);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = article.PublicationTimestampUtc.AddDays(14)
        };

        await _cache.SetStringAsync($"article:{article.PublicId}", value, options, ct);
    }
}