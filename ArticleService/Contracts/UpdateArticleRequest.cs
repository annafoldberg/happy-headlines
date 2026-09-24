using System.ComponentModel.DataAnnotations;

namespace ArticleService.Contracts;

/// <summary>
/// Represents the data used to update a published article.
/// </summary>
public class UpdateArticleRequest
{
    [RegularExpression(@".*\S.*", ErrorMessage = "Author cannot be empty or whitespace.")]
    [MaxLength(100)]
    public string? Author { get; set; }

    [RegularExpression(@".*\S.*", ErrorMessage = "Title cannot be empty or whitespace.")]
    [MaxLength(200)]
    public string? Title { get; set; }

    [RegularExpression(@".*\S.*", ErrorMessage = "Content cannot be empty or whitespace.")]
    public string? Content { get; set; }
}