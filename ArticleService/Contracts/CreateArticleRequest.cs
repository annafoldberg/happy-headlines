using System.ComponentModel.DataAnnotations;

namespace ArticleService.Contracts;

/// <summary>
/// Represents the data used to create a published article.
/// </summary>
public class CreateArticleRequest
{
    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;
}