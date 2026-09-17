namespace ArticleService.Persistence.Configuration;

/// <summary>
/// Configuration options for database host.
/// </summary>
public sealed class DatabaseHostOptions
{
    public string Host { get; init; } = string.Empty;
}
