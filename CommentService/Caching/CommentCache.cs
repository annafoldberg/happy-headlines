using System.Text.Json;
using CommentService.Entities;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Diagnostics.Metrics;

namespace CommentService.Caching;

public class CommentCache : ICommentCache
{
    private readonly IDistributedCache _cache;
    private readonly IDatabase _redis;
    private const string LruKey = "comments:lru";
    private static readonly Meter Meter = new("CommentService.Caching");
    private static readonly Counter<long> CacheHits = Meter.CreateCounter<long>("comment_cache_hits");
    private static readonly Counter<long> CacheMisses = Meter.CreateCounter<long>("comment_cache_misses");

    public CommentCache(IDistributedCache cache, IConnectionMultiplexer connection)
    {
        _cache = cache;
        _redis = connection.GetDatabase();
    }

    public async Task<IReadOnlyList<Comment>?> GetByArticleIdAsync(Guid articleId, CancellationToken ct)
    {
        var value = await _cache.GetStringAsync($"comments:{articleId}", ct);

        if (value is null)
        {
            CacheMisses.Add(1);
            return null;
        }

        CacheHits.Add(1);

        await _redis.SortedSetAddAsync(LruKey, articleId.ToString(), DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        return JsonSerializer.Deserialize<List<Comment>>(value);
    }

    public async Task SetAsync(Guid articleId, IReadOnlyList<Comment> comments, CancellationToken ct)
    {
        var value = JsonSerializer.Serialize(comments);

        await _cache.SetStringAsync($"comments:{articleId}", value, ct);

        await _redis.SortedSetAddAsync(LruKey, articleId.ToString(), DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var count = await _redis.SortedSetLengthAsync(LruKey);

        if (count > 30)
        {
            // Remove article with lowest timestamp
            var leastRecentlyUsedArticles = await _redis.SortedSetRangeByRankAsync(LruKey, 0, 0);
            
            var evictedArticleId = leastRecentlyUsedArticles.First();

            await _cache.RemoveAsync($"comments:{evictedArticleId}", ct);

            await _redis.SortedSetRemoveAsync(LruKey, evictedArticleId);
        }
    }

    public async Task RemoveAsync(Guid articleId, CancellationToken ct)
    {
        await _cache.RemoveAsync($"comments:{articleId}", ct);

        await _redis.SortedSetRemoveAsync(LruKey, articleId.ToString());
    }
}