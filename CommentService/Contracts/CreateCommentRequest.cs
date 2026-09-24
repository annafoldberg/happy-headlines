using System.ComponentModel.DataAnnotations;

namespace CommentService.Contracts;

public class CreateCommentRequest
{
    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;
}