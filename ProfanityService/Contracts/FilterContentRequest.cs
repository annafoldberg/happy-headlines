using System.ComponentModel.DataAnnotations;

namespace ProfanityService.Contracts;

public class FilterContentRequest
{
    [Required]
    public string Content { get; set; } = string.Empty;
}