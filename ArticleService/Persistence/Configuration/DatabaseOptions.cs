namespace ArticleService.Persistence.Configuration;

/// <summary>
/// Configuration options for database connection.
/// </summary>
public sealed class DatabaseOptions
{
    public const string SectionName = "ArticleDatabase";

    public DatabaseHostOptions Africa { get; init; } = new();
    public DatabaseHostOptions Antarctica { get; init; } = new();
    public DatabaseHostOptions Asia { get; init; } = new();
    public DatabaseHostOptions Europe { get; init; } = new();
    public DatabaseHostOptions NorthAmerica { get; init; } = new();
    public DatabaseHostOptions Oceania { get; init; } = new();
    public DatabaseHostOptions SouthAmerica { get; init; } = new();
    public DatabaseHostOptions Global { get; init; } = new();
        
    public string Name { get; init; } = string.Empty;
    public string User { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}