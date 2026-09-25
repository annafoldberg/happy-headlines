using System.ComponentModel.DataAnnotations;

namespace DraftService.Contracts;

public class DraftRequest
{
    [MaxLength(100)]
    public string? Author { get; set; }

    [MaxLength(200)]
    public string? Title { get; set; }

    public string? Content { get; set; }
}