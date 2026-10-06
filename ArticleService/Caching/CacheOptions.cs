namespace ArticleService.Caching;

public class CacheOptions
{
    public const string SectionName = "ArticleCache";
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 6379;
}