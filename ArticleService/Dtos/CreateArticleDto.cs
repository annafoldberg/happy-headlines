namespace ArticleService.Dtos;

/// <summary>
/// Represents the data used to create a published article.
/// </summary>
public class CreateArticleDto
{
    public string Author { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}