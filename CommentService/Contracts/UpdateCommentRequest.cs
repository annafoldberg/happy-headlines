using System.ComponentModel.DataAnnotations;

namespace CommentService.Contracts;

public class UpdateCommentRequest
{
    [RegularExpression(@".*\S.*", ErrorMessage = "Author cannot be empty or whitespace.")]
    [MaxLength(100)]
    public string? Author { get; set; }

    [RegularExpression(@".*\S.*", ErrorMessage = "Content cannot be empty or whitespace.")]
    [MaxLength(2000)]
    public string? Content { get; set; }
}