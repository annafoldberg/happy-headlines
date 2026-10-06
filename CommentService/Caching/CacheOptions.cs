namespace CommentService.Caching;

public class CacheOptions
{
    public const string SectionName = "CommentCache";
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 6379;
}