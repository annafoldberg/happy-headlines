using System.ComponentModel.DataAnnotations;

namespace ProfanityService.Contracts;

public class FilterCommentRequest
{
    [Required]
    public string Comment { get; set; } = string.Empty;
}