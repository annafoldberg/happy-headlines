namespace ArticleService.Dtos;

/// <summary>
/// Represents the data used to update a published article.
/// </summary>
public class UpdateArticleDto
{
    public string? Author { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
}