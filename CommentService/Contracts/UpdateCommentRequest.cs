using System.ComponentModel.DataAnnotations;

namespace CommentService.Contracts;

public class UpdateCommentRequest
{
    [RegularExpression(@".*\S.*", ErrorMessage = "Content cannot be empty or whitespace.")]
    [MaxLength(2000)]
    public string? Content { get; set; }
}